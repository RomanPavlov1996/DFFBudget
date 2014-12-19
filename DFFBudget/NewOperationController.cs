using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace DFFBudget
{
	public partial class NewOperationController : UIViewController
	{
		public NewOperationController() : base("NewOperationController", null)
		{
			TabBarItem = new UITabBarItem("Добавить", UIImage.FromFile("Icons/plus.png"), 1);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			txtSum.Started += ShowKeyboard;
			btnHideKeyboard.TouchUpInside += HideKeyboard;
			btnAddOperation.TouchUpInside += AddOperation;
			btnChangeCategories.TouchUpInside += ChangeCategories;

			segCategory.RemoveAllSegments();
			for (int i = 0; i < Global.Categories.Count; i++)
			{
				segCategory.InsertSegment(Global.Categories[i].Title, i, true);
			}
			segCategory.SelectedSegment = 0;
		}

		void ChangeCategories(object sender, EventArgs e)
		{
			AppDelegate.CategoriesPage(delegate
			{
				Refresh();
			});
		}

		public void Refresh()
		{
			Global.PrepareCategories();

			segCategory.RemoveAllSegments();
			for (int i = 0; i < Global.Categories.Count; i++)
			{
				segCategory.InsertSegment(Global.Categories[i].Title, i, true);
			}
			segCategory.SelectedSegment = 0;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//txtSum.BecomeFirstResponder();
			btnHideKeyboard.Hidden = true;
		}

		void AddOperation(object sender, EventArgs e)
		{
			if (txtSum.Text != "")
			{
				var operation = new Operation(){ Sum = int.Parse(txtSum.Text), CategoryId = segCategory.SelectedSegment, Datetime = DateTime.Now };
				txtSum.Text = "";
				Global.Outcomes.Add(operation);

				Task.Factory.StartNew(
					() =>
					{
						DataLayer.AddOperation(operation, "outcome");
					}
				);
			}
		}

		void ShowKeyboard(object sender, EventArgs e)
		{
			btnHideKeyboard.Hidden = false;
		}

		void HideKeyboard(object sender, EventArgs e)
		{
			txtSum.EndEditing(true);
			btnHideKeyboard.Hidden = true;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationController.SetNavigationBarHidden(true, animated);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			this.NavigationController.SetNavigationBarHidden(false, animated);
		}
	}
}

