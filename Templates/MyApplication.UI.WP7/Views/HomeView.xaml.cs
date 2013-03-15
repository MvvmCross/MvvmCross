using Cirrious.MvvmCross.WindowsPhone.Views;
using MyApplication.Core.ViewModels;

namespace MyApplication.UI.WP7.Views
{
    public partial class HomeView 
        : MvxPhonePage
    {
        public new HomeViewModel ViewModel
        {
            get { return (HomeViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public HomeView()
        {
            InitializeComponent();
        }
    }
}