using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.IO;
using System.Drawing;
using MonoTouch;
using System.Threading;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreImage;
using System.Net.Http;

namespace DFFBudget
{
	public class Application
	{
		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
	// The name AppDelegate is referenced in the MainWindow.xib file.
	[Register("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public static UIWindow window;
		public static UITabBarController nav;
		public static UINavigationController navigation;
		public static UINavigationController newOperationController;

		static void MyHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception e = (Exception)args.ExceptionObject;
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri("http://dutyfreeflowers.ru/api/");
			var response = client.PutAsync(String.Format("log?method={0}&msg={1}&token={2}", "POST", "Exception: " + e.Message, Global.Token), null).Result;
			Console.WriteLine("Exception: " + e.StackTrace);
		}

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

			UIView.Appearance.TintColor = UIColor.FromRGB(255, 51, 0);
			DataLayer.Prepare();

			window = new UIWindow(UIScreen.MainScreen.Bounds);

			navigation = new UINavigationController();

			nav = new UITabBarController();
			nav.CustomizableViewControllers = new UIViewController [0];

			window.RootViewController = new Auth();
			window.MakeKeyAndVisible();

			return true;
		}
	}
}
