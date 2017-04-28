using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Presenter.WindowsUWP;
using MvvmCross.Platform;

namespace MvxBindingsExample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
                        
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsWindowsUWPPagePresenter;

            LoadApplication(presenter.MvxFormsApp);
        }
    }
}
