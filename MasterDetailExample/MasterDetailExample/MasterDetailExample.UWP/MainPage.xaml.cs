using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Presenter.WindowsUWP;
using MvvmCross.Platform;
using Xamarin.Forms.Platform.UWP;

namespace MasterDetailExample.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : WindowsPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindowsUWPMasterDetailPagePresenter;

            LoadApplication(presenter.MvxFormsApp);
        }
    }
}
