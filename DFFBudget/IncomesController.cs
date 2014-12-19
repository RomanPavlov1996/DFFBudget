using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.IO;
using System.Drawing;
using MonoTouch;
using MonoTouch.CoreGraphics;
using System.Threading.Tasks;

namespace DFFBudget
{
	public partial class AppDelegate
	{
		public class IncomesController : UINavigationController
		{
			RootElement root;
			DialogViewController dv;

			public override void ViewDidAppear(bool animated)
			{
				base.ViewDidAppear(animated);
				navigation = this;
				GroupByDate();
			}

			public IncomesController()
			{
				TabBarItem = new UITabBarItem("Доходы", UIImage.FromFile("Icons/internal.png"), 1);

				dv = new DialogViewController(root);
				dv.RefreshControl = new UIRefreshControl();
				dv.RefreshControl.ValueChanged += Refresh;
				dv.Root = new RootElement("Доходы");
				root = dv.Root;

				var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add);
				addButton.Clicked += delegate
				{					
					NewIncomePage(delegate
					{
						GroupByDate();
					});
				};
				dv.NavigationItem.RightBarButtonItem = addButton;

				PushViewController(dv, false);
			}

			public void Refresh(object sender, EventArgs e)
			{
				Task.Factory.StartNew(
					() =>
					{
						Global.Incomes = DataLayer.GetOperations("income");
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
				if (Global.Incomes != null)
				{
					root.Clear();

					//Global.Incomes = Global.Incomes.OrderByDescending(op => op.Datetime).ToList();
					var groups = Global.Incomes.GroupBy(op => op.Datetime).ToList();

					foreach (var cGroup in groups.OrderByDescending(op => op.Key).ToList())
					{
						root.Add(new Section(cGroup.Key.ToString("dd MMMM yyyy")));

						foreach (var cOp in cGroup.OrderBy(op => op.Datetime).ToList())
						{
							root.Last().Add(new StringElement(cOp.Sum.ToString() + "руб.", () => OperationPage(cOp, () =>
							{
								GroupByDate();
							}, "income")));
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
