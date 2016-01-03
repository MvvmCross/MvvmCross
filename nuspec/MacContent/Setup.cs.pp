using MonoMac.Foundation;
using MonoMac.AppKit;
using MvvmCross.Core.ViewModels;
using MvvmCross.Mac.Platform;
using MvvmCross.Mac.Views.Presenters;
using CrossCore.Platform;

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
