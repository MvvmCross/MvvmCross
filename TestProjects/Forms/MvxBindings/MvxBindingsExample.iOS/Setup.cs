using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using UIKit;
using Xamarin.Forms;

namespace MvxBindingsExample.iOS
{
    public class Setup : MvxIosSetup
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

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();

            return new MvxFormsIosPagePresenter(Window, xamarinFormsApp);
        }

        protected override MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxFormsBindingBuilder();
        }
    }
}
