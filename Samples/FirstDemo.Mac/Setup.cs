using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;

namespace FirstDemo.Mac
{
	public class Setup : MvxMacSetup
	{
		public Setup (MvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
			: base(applicationDelegate, presenter)
		{

		}

		protected override IMvxApplication CreateApp ()
		{
			return new FirstDemo.Core.App ();
		}
	}
}

