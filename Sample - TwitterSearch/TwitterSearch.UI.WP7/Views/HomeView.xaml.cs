using Cirrious.MvvmCross.WindowsPhone.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.WP7.Views
{
    public partial class HomeView
        : MvxPhonePage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}