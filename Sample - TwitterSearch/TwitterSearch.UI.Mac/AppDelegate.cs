using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.Mac.Platform;

namespace TwitterSearch.UI.Mac
{
	public partial class AppDelegate : MvxApplicationDelegate
	{
		MainWindowController mainWindowController;
		
		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();

			var presenter = new MvxMacViewPresenter(this, mainWindowController.Window);
			var setup = new Setup(this, presenter);
			setup.Initialize();

			mainWindowController.Window.MakeKeyAndOrderFront (this);
		}
	}
}

