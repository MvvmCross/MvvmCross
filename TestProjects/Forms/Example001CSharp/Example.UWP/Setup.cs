using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using Windows.ApplicationModel.Activation;
using XamlControls = Windows.UI.Xaml.Controls;
using MvvmCross.WindowsUWP.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.WindowsUWP.Platform;
using MvvmCross.Forms.Presenter.WindowsUWP;

namespace Example.UWP
{
    public class Setup : MvxWindowsSetup
    {
        private readonly LaunchActivatedEventArgs _launchActivatedEventArgs;

        public Setup(XamlControls.Frame rootFrame, LaunchActivatedEventArgs e) : base(rootFrame)
        {
            _launchActivatedEventArgs = e;
        }

        protected override IMvxApplication CreateApp()
        {
            return new Example.App();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Forms.Init(_launchActivatedEventArgs);

            var xamarinFormsApp = new MvxFormsApp();
            var presenter = new MvxFormsWindowsUWPPagePresenter(rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}