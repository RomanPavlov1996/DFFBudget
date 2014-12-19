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
		public static void NewCategoryPage(Action refresh)
		{
			var title = new EntryElement("Название", "", "");
			var root = new RootElement("Категория")
			{
				new Section()
				{
					title
				}
			};

			var dv = new DialogViewController(root, true);

			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done);
			doneButton.Clicked += delegate
			{
				Task.Factory.StartNew(
					() =>
					{
						int id = DataLayer.AddCategory();
						Category newCategory = new Category(){ Id = id, Title = title.Value };
						DataLayer.PutCategory(newCategory);
					}
				).ContinueWith(
					t =>
					{
						refresh();
						newOperationController.PopViewControllerAnimated(true);
					}, TaskScheduler.FromCurrentSynchronizationContext()
				);
			};
			dv.NavigationItem.RightBarButtonItem = doneButton;

			newOperationController.PushViewController(dv, true);
		}
	}
}

