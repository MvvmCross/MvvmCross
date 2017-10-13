using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Platform;

namespace MvxBindingsExample.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
                        
            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsUwpViewPresenter;

            LoadApplication(presenter.FormsApplication);
        }
    }
}
