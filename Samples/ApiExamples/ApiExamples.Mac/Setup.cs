using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;

namespace ApiExamples.Mac
{
	public class Setup : MvxMacSetup
	{
		public Setup (MvxApplicationDelegate applicationDelegate, NSWindow window)
			: base(applicationDelegate, window)
		{

		}

		protected override IMvxApplication CreateApp ()
		{
			return new ApiExamples.Core.App ();
		}
	}
}

