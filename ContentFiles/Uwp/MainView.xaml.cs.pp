using MvvmCross.Platforms.Uap.Views;

namespace $rootnamespace$.Views
{
    public sealed partial class MainView : MvxWindowsPage
    {
        private MainViewModel Vm => (MainViewModel) ViewModel;

        public MainView()
        {
            this.InitializeComponent();
        }
    }
}
