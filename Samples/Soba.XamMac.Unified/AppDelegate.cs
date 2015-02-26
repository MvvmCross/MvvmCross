using System.Drawing;
using AppKit;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Foundation;

namespace Soba.XamMac.Unified
{
	public partial class AppDelegate : MvxApplicationDelegate
	{
		NSWindow _window;

		public AppDelegate()
		{
		}

		public override void DidFinishLaunching (NSNotification notification)
		{
			_window = new NSWindow (new RectangleF(200,200,400,400), NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled,
				NSBackingStore.Buffered, false, NSScreen.MainScreen);

			var presenter = new MvxMacViewPresenter (this, _window);
			var setup = new Setup (this, presenter);
			setup.Initialize ();

			var startup = Mvx.Resolve<IMvxAppStart> ();
			startup.Start ();

			_window.MakeKeyAndOrderFront (this);

			return;
		}
	}
}

