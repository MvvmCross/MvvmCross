using AppKit;
using Foundation;
using MvvmCross.Platforms.Mac.Core;
using MvvmCross.Platforms.Mac.Presenters;
using MvvmCross.ViewModels;

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
    }
}
