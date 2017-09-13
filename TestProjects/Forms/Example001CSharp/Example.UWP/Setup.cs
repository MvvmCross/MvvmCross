using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Core.Views;
using Xamarin.Forms;
using Windows.ApplicationModel.Activation;
using MvvmCross.Uwp.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Uwp.Platform;
using MvvmCross.Forms.Uwp.Presenters;
using XamlControls = Windows.UI.Xaml.Controls;

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

            var xamarinFormsApp = new MvxFormsApplication();
            var presenter = new MvxFormsUwpPagePresenter(rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}