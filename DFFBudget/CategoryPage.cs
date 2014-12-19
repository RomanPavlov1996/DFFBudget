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
		public static void CategoryPage(Category category, Action refresh)
		{
			var title = new EntryElement("Название", "", category.Title);
			var root = new RootElement("Категория")
			{
				new Section()
				{
					title
				}
			};

			var dv = new DialogViewController(root, true);
			dv.ViewDisappearing += delegate
			{
				category.Title = title.Value;

				DataLayer.PutCategory(category);
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
						Task.Factory.StartNew(
							() =>
							{
								DataLayer.DeleteCategory(category);
							}
						).ContinueWith(
							t =>
							{
								newOperationController.PopViewControllerAnimated(true);
							}, TaskScheduler.FromCurrentSynchronizationContext()
						);
					}
				};
				alert.Show();				
			};
			dv.NavigationItem.RightBarButtonItem = deleteButton;

			newOperationController.PushViewController(dv, true);
		}
	}
}

