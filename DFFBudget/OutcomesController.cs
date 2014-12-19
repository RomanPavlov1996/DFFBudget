using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.IO;
using System.Drawing;
using MonoTouch;
using System.Threading.Tasks;
using System.Collections;

namespace DFFBudget
{
	public partial class AppDelegate
	{
		public class OutcomesController : UINavigationController
		{
			RootElement root;
			DialogViewController dv;

			public override void ViewDidAppear(bool animated)
			{
				base.ViewDidAppear(animated);
				navigation = this;
				GroupByDate();
			}

			public OutcomesController()
			{
				TabBarItem = new UITabBarItem("Расходы", UIImage.FromFile("Icons/external.png"), 1);
				
				root = new RootElement("Расходы");
				dv = new DialogViewController(root);
				dv.RefreshControl = new UIRefreshControl();
				dv.RefreshControl.ValueChanged += Refresh;

				PushViewController(dv, false);
			}

			private void Refresh(object sender, EventArgs e)
			{
				Task.Factory.StartNew(
					() =>
					{
						Global.Outcomes = DataLayer.GetOperations("outcome");
					}
				).ContinueWith(
					t =>
					{
						GroupByDate();

						dv.RefreshControl.EndRefreshing();
						dv.ReloadComplete();
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
			}

			void GroupByDate()
			{
				if (Global.Outcomes != null)
				{
					root.Clear();

					Global.Outcomes = Global.Outcomes.OrderByDescending(op => op.Datetime).ToList();
					var groups = Global.Outcomes.GroupBy(op => op.Datetime).ToList();

					foreach (var cGroup in groups)
					{
						root.Add(new Section(cGroup.Key.ToString("dd MMMM yyyy")));

						foreach (var cOp in cGroup)
						{
							root.Last().Add(new StringElement(cOp.Sum.ToString() + "руб.", () => OperationPage(cOp, () =>
							{
								GroupByDate();
							}, "outcome")));
						}
					}
				}
				else
				{
					root.Add(new Section("Нет записей"));
				}
			}
		}
	}
}
