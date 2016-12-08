using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Forms.Presenter.WindowsPhone;
using MvvmCross.Core.Views;
using MvvmCross.WindowsPhone.Platform;
using MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using Xamarin.Forms;

namespace Example.WindowsPhone
{
    public class Setup : MvxPhoneSetup
    {
        public Setup(PhoneApplicationFrame rootFrame) : base(rootFrame)
        {
        }

        protected override MvvmCross.Core.ViewModels.IMvxApplication CreateApp()
        {
            return new Example.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override IMvxPhoneViewPresenter CreateViewPresenter(PhoneApplicationFrame rootFrame)
        {
            Forms.Init();

            var xamarinFormsApp = new MvxFormsApp();
            var presenter = new MvxFormsWindowsPhonePagePresenter(rootFrame, xamarinFormsApp);
            Mvx.RegisterSingleton<IMvxViewPresenter>(presenter);

            return presenter;
        }
    }
}