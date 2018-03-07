using MvvmCross.Uwp.Views;

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
