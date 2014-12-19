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
		public static void OperationPage(Operation operation, Action refresh, string type)
		{
			var delete = false;
			var date = new DateElement("Дата", operation.Datetime);
			var category = new RootElement("Категория", new RadioGroup(operation.CategoryId)){ Global.CategoriesRoot };
			var sum = new EntryElement("Сумма", "", operation.Sum.ToString())
			{
				KeyboardType = UIKeyboardType.DecimalPad
			};
			var comment = new EntryElement("Комментарий", "", operation.Comment)
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
				if (!delete)
				{
					operation.Datetime = date.DateValue;
					operation.CategoryId = category.RadioSelected;
					operation.Sum = int.Parse(sum.Value);
					operation.Comment = comment.Value;

					Task.Factory.StartNew(
						() =>
						{
							DataLayer.PutOperation(operation, type);
						}
					).ContinueWith(
						t =>
						{
						}, TaskScheduler.FromCurrentSynchronizationContext()
					);
				}
				refresh();
			};

			var deleteButton = new UIBarButtonItem(UIBarButtonSystemItem.Trash);
			deleteButton.Clicked += delegate
			{
				UIAlertView alert = new UIAlertView("Удаление", "Вы точно хотите удалить?", null, "Да", new string[] { "Нет" });
				alert.Clicked += (s, b) =>
				{
					if (b.ButtonIndex == 0)
					{
						delete = true;

						if (type == "income")
							Global.Incomes.Remove(operation);
						else
							Global.Outcomes.Remove(operation);

						Task.Factory.StartNew(
							() =>
							{
								DataLayer.DeleteOperation(operation, type);
							}
						).ContinueWith(
							t =>
							{
							}, TaskScheduler.FromCurrentSynchronizationContext()
						);

						navigation.PopViewControllerAnimated(true);
					}
				};
				alert.Show();
			};
			dv.NavigationItem.RightBarButtonItem = deleteButton;

			navigation.PushViewController(dv, true);
		}
	}
}

