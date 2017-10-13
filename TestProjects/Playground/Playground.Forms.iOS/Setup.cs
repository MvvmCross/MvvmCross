using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.iOS;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.Forms.Platform;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace Playground.Forms.iOS
{
    public class Setup : MvxFormsIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override MvxFormsApplication CreateFormsApplication()
        {
            return new Core.FormsApp();
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}
