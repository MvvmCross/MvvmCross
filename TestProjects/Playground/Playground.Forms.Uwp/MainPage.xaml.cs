using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Uwp.Presenters;
using MvvmCross.Forms.Views;
using MvvmCross.Platform;
using MvvmCross.Forms.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Playground.Forms.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            InitializeComponent();

            var start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}