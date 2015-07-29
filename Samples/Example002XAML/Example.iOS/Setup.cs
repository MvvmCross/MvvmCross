using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Touch.Views.Presenters;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using Xamarin.Forms;
using Cirrious.MvvmCross.Forms.Presenter.Touch;
using Cirrious.MvvmCross.Forms.Presenter.Core;

namespace Example.iOS
{
    public class Setup : MvxTouchSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxTouchViewPresenter CreatePresenter()
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();

            return new MvxFormsTouchPagePresenter(Window, xamarinFormsApp);
        }
    }
}
