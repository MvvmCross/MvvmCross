using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;

using Xamarin.Forms;

using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using MvvmCross.Platform;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp.Presenters;
using PageRendererExample.ViewModels;

namespace PageRendererExample.UI.Uwp
{
    class MvvmSetup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;
        public MvxFormsApp MvxFormsApp { get; private set; }
        public MvvmSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            return new MvvmApp();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Forms.Init(_launchActivatedEventArgs);

            MvxFormsApp = new PageRendererExampleApp();

            var presenter = new MvxFormsUwpPagePresenter(rootFrame, MvxFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);
            Mvx.LazyConstructAndRegisterSingleton<IImageHolder, ImageHolder>();

            return presenter;
        }
    }
}
