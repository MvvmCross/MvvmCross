using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.Forms.Presenter.Windows81;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Xamarin.Forms;
using Cirrious.MvvmCross.WindowsCommon.Views;
using XamlControls = Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;

namespace Example.W81
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
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxWindowsViewPresenter CreateViewPresenter(IMvxWindowsFrame rootFrame)
        {
            Forms.Init(_launchActivatedEventArgs);

            var xamarinFormsApp = new MvxFormsApp();
            var presenter = new MvxFormsWindows81PagePresenter(  rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}