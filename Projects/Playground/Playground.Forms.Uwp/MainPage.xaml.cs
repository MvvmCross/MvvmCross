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
            // This is required so that navigating to a native page and back again doesn't
            // reload XF
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Loaded -= MainPage_Loaded;

            var start = Mvx.Resolve<IMvxAppStart>();
            await start.StartAsync();

            var presenter = Mvx.Resolve<IMvxFormsViewPresenter>() as MvxFormsUwpViewPresenter;
            LoadApplication(presenter.FormsApplication);
        }
    }
}
