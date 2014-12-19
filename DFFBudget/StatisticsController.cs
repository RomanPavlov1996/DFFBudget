using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.IO;
using System.Drawing;
using MonoTouch;
using System.Runtime.InteropServices;

namespace DFFBudget
{
	public partial class AppDelegate
	{
		public class StatisticsController : UINavigationController
		{
			RootElement root;
			DialogViewController dv;
			string[] Month = new string[]
			{
				"",
				"Январь",
				"Февраль",
				"Март",
				"Апрель",
				"Май",
				"Июнь",
				"Июль",
				"Август",
				"Сентябрь",
				"Октябрь",
				"Ноябрь",
				"Декабрь"
			};

			public StatisticsController()
			{
				TabBarItem = new UITabBarItem("Статистика", UIImage.FromFile("Icons/chart.png"), 1);

				root = new RootElement("Статистика");
				dv = new DialogViewController(root);
				MakeStatistics();
				PushViewController(dv, false);
			}

			public override void ViewDidAppear(bool animated)
			{
				base.ViewDidAppear(animated);
				MakeStatistics();
			}

			private void MakeStatistics()
			{
				root.Clear();

				if (Global.Outcomes != null)
				{
					Section opSection = new Section("null");

					var finishDate = DateTime.Now.AddMonths(-6);

					for (DateTime cDate = DateTime.Now; cDate > finishDate; cDate = cDate.AddMonths(-1))
					{
						int incomes = 0;
						int outcomes = 0;
						foreach (var cOutcome in Global.Outcomes)
						{
							if (cOutcome.Datetime.Year == cDate.Year && cOutcome.Datetime.Month == cDate.Month)
							{
								outcomes += cOutcome.Sum;
							}
						}
						foreach (var cIncome in Global.Incomes)
						{
							if (cIncome.Datetime.Year == cDate.Year && cIncome.Datetime.Month == cDate.Month)
							{
								incomes += cIncome.Sum;
							}
						}

						if (incomes + outcomes != 0)
						{
							opSection = new Section(String.Format("{0} {1}", Month[cDate.Month], cDate.Year))
							{
								new StringElement("Расходы", outcomes.ToString()),
								new StringElement("Доходы", incomes.ToString()),
								new StringElement("Баланс", (incomes - outcomes).ToString())
							};
							root.Add(opSection);
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

