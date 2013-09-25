using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using System.Drawing;

namespace $rootnamespace$
{
	public partial class AppDelegate : MvxApplicationDelegate
	{
		MainWindowController mainWindowController;

		public AppDelegate()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
    
			var setup = new Setup (this, mainWindowController.Window);
			setup.Initialize ();

			var startup = Mvx.Resolve<IMvxAppStart> ();
			startup.Start ();

			mainWindowController.Window.MakeKeyAndOrderFront (this);

			return;
		}
	}
}

