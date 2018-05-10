using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace $rootnamespace$.Views
{
    [MvxViewFor(typeof(HomeViewModel))]
    public partial class HomeView : MvxWpfView
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}
