using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using UIKit;
using Xamarin.Forms;
using MvvmCross.Platform;

namespace MasterDetailExample.iOS
{
    public class Setup : MvxIosSetup
    {
        public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }        

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApplication();

            return new MvxFormsIosMasterDetailPagePresenter(Window, xamarinFormsApp);
        }
    }
}
