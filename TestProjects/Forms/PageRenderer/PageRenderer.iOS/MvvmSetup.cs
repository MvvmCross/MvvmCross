using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.iOS.Presenters;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using PageRendererExample.ViewModels;
using UIKit;
using Xamarin.Forms;

namespace PageRendererExample.UI.iOS
{
    public class MvvmSetup : MvxIosSetup
    {
        public MvvmSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate,
            window)
        {
        }

        public MvxFormsApp MvxFormsApp { get; private set; }

        protected override IMvxApplication CreateApp()
        {
            return new MvvmApp();
        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            Forms.Init();

            MvxFormsApp = new PageRendererExampleApp();

            var presenter = new MvxFormsIosPagePresenter(Window, MvxFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();

            return presenter;
        }
    }
}