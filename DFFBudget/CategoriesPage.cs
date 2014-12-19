using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.ComponentModel.Design;

namespace DFFBudget
{
	public partial class AppDelegate
	{
		static RootElement root;

		public static void CategoriesPage(Action refresh)
		{
			var section = new Section();

			foreach (Category cCategory in Global.Categories)
			{
				section.Add(new StringElement(cCategory.Title, delegate
				{
					CategoryPage(cCategory, delegate
					{
						Refresh();
					});
				}));
			}

			root = new RootElement("Категории")
			{
				section
			};

			var dv = new DialogViewController(root, true);
			dv.ViewDisappearing += delegate
			{
				refresh();
			};

			var addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add);
			addButton.Clicked += delegate
			{
				NewCategoryPage(delegate
				{
					Refresh();
				});
			};
			dv.NavigationItem.RightBarButtonItem = addButton;

			newOperationController.PushViewController(dv, true);
		}

		private static void Refresh()
		{
			Global.PrepareCategories();

			var section = new Section();

			foreach (Category cCategory in Global.Categories)
			{
				section.Add(new StringElement(cCategory.Title, delegate
				{
					CategoryPage(cCategory, delegate
					{
						Refresh();
					});
				}));
			}

			root.Clear();
			root.Add(section);

			if (root.Count == 0)
			{
				root.Add(new Section("Нет записей"));
			}
		}
	}
}

