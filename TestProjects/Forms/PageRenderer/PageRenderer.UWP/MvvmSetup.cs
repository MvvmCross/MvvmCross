using Windows.ApplicationModel.Activation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Core;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Platform;
using MvvmCross.Uwp.Platform;
using MvvmCross.Uwp.Views;
using PageRendererExample.ViewModels;
using Xamarin.Forms;
using XamlControls = Windows.UI.Xaml.Controls;

namespace PageRendererExample.UI.Uwp
{
    internal class MvvmSetup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;

        public MvvmSetup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        public MvxFormsApp MvxFormsApp { get; private set; }

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