
using UIKit;
using Xamarin.Forms;

using MvvmCross.iOS.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Forms.Presenter.iOS;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using PageRendererExample.ViewModels;

namespace PageRendererExample.UI.iOS
{
    public class MvvmSetup : MvxIosSetup
    {
        public MvxFormsApp MvxFormsApp { get; private set; }
        public MvvmSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {
        }

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

