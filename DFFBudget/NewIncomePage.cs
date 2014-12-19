using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace DFFBudget
{
	public partial class AppDelegate
	{
		public static void NewIncomePage(Action refresh)
		{
			var operation = new Operation();
			string type = "income";
			var date = new DateElement("Дата", DateTime.Now);
			var category = new RootElement("Категория", new RadioGroup(0)){ Global.CategoriesRoot };
			var sum = new EntryElement("Сумма", "", "")
			{
				KeyboardType = UIKeyboardType.NumberPad
			};
			var comment = new EntryElement("Комментарий", "", "")
			{
				ReturnKeyType = UIReturnKeyType.Done
			};

			var root = new RootElement("Подробности")
			{
				new Section()
				{
					date,
					category,
					sum,
					comment
				}
			};

			var dv = new DialogViewController(root, true);
			dv.ViewDisappearing += delegate
			{
				refresh();
			};

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += delegate
			{
				if (sum.Value != "")
				{

					operation.Datetime = date.DateValue;
					operation.CategoryId = category.RadioSelected;
					operation.Sum = int.Parse(sum.Value);
					operation.Comment = comment.Value;
					Global.Incomes.Add(operation);

					Task.Factory.StartNew(
						() =>
						{
							operation.Id = DataLayer.AddOperation(operation, type);
						}
					).ContinueWith(
						t =>
						{
							Task.Factory.StartNew(
								() =>
								{
									DataLayer.PutOperation(operation, type);
								}
							);
						}, TaskScheduler.FromCurrentSynchronizationContext()
					);
					navigation.PopViewControllerAnimated(true);
				}
				else
				{
					UIAlertView alert = new UIAlertView("", "Заполните поле \"Сумма\"", null, "OK", null);
					alert.Show();
				}
			};
			dv.NavigationItem.RightBarButtonItem = doneButton;

			navigation.PushViewController(dv, true);
		}
	}
}

