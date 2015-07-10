using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Cirrious.MvvmCross.Forms.Presenter.Core;
using Cirrious.MvvmCross.Forms.Presenter.WindowsStore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Xamarin.Forms;
using W = Windows.UI.Xaml.Controls;
using Cirrious.MvvmCross.WindowsCommon.Views;

namespace Example.WindowStore
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(W.Frame rootFrame) : base(rootFrame)
        {
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
            Forms.Init(null);

            var xamarinFormsApp = new MvxFormsApp();
            var presenter = new MvxFormsWindowsStorePagePresenter(  rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}