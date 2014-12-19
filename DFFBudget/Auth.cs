using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;

namespace DFFBudget
{
	public partial class Auth : UIViewController
	{
		public Auth() : base("Auth", null)
		{
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var email = NSUserDefaults.StandardUserDefaults.StringForKey("Email");
			if (email != null)
			{
				txtEmail.Text = email;
			}
			var password = NSUserDefaults.StandardUserDefaults.StringForKey("Password");
			if (password != null)
			{
				txtPassword.Text = password;
			}

			txtPassword.EditingDidEndOnExit += delegate
			{
				Login();
			};

			txtEmail.EditingDidEndOnExit += delegate
			{
				txtPassword.BecomeFirstResponder();
			};
			
			btnLogin.TouchUpInside += delegate
			{
				Login();
			};


		}

		private void Login()
		{
			var loadingOverlay = new LoadingOverlay(UIScreen.MainScreen.Bounds);
			View.Add(loadingOverlay);
			var email = txtEmail.Text;
			var password = txtPassword.Text;

			Task.Factory.StartNew(
				() =>
				{
					Global.Token = DataLayer.Login(email, password);
					Global.PrepareCategories();
					Global.Outcomes = DataLayer.GetOperations("outcome");
					Global.Incomes = DataLayer.GetOperations("income");
				}
			).ContinueWith(
				t =>
				{
					if (Global.Token == null)
					{
						UIAlertView alert = new UIAlertView("Ошибка", "Сервер не отвечает :(", null, "OK", null);
						alert.Show();
					}
					else if (Global.Token == "")
					{
						UIAlertView alert = new UIAlertView("Ошибка", "Неправильные логин/пароль", null, "OK", null);
						alert.Show();
					}
					else
					{
						NSUserDefaults.StandardUserDefaults.SetString(txtEmail.Text, "Email");
						NSUserDefaults.StandardUserDefaults.SetString(txtPassword.Text, "Password");
						NSUserDefaults.StandardUserDefaults.Init();

						AppDelegate.window.RootViewController = new Auth();
						AppDelegate.newOperationController = new UINavigationController(new NewOperationController());
						AppDelegate.nav.ViewControllers = new UIViewController []
						{
							AppDelegate.newOperationController,
							new AppDelegate.IncomesController(),
							new AppDelegate.OutcomesController(),
							new AppDelegate.StatisticsController()
						};

						AppDelegate.window.RootViewController = AppDelegate.nav;
						AppDelegate.window.MakeKeyAndVisible();
					}

					loadingOverlay.Hide();
				}, TaskScheduler.FromCurrentSynchronizationContext()
			);
		}
	}
}