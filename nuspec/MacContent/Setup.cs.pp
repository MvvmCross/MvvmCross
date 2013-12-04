using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Mac.Platform;
using Cirrious.MvvmCross.Mac.Views.Presenters;
using Cirrious.CrossCore.Platform;

namespace $rootnamespace$
{
	public class Setup : MvxMacSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, NSWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}
		
		protected override IMvxTrace CreateDebugTrace()
		{
				return new DebugTrace();
		}
	}
}