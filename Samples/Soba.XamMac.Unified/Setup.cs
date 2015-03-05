using AppKit;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Foundation;

namespace Soba.XamMac.Unified
{
	public class Setup : MvxMacSetup
	{
		public Setup (MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
			: base(applicationDelegate, presenter)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Soba.Core.App ();
		}
	}
}