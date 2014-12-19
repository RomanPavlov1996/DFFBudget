// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace DFFBudget
{
	[Register ("NewOperationController")]
	partial class NewOperationController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton btnAddOperation { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnChangeCategories { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton btnHideKeyboard { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl segCategory { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField txtSum { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnAddOperation != null) {
				btnAddOperation.Dispose ();
				btnAddOperation = null;
			}

			if (btnHideKeyboard != null) {
				btnHideKeyboard.Dispose ();
				btnHideKeyboard = null;
			}

			if (segCategory != null) {
				segCategory.Dispose ();
				segCategory = null;
			}

			if (txtSum != null) {
				txtSum.Dispose ();
				txtSum = null;
			}

			if (btnChangeCategories != null) {
				btnChangeCategories.Dispose ();
				btnChangeCategories = null;
			}
		}
	}
}
