using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace $rootnamespace$
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override MvvmCross.Forms.Platform.MvxFormsApplication CreateFormsApplication()
        {
            return new Core.App();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.CoreApp();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
