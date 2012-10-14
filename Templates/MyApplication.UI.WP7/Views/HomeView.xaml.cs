using Cirrious.MvvmCross.WindowsPhone.Views;
using MyApplication.Core.ViewModels;

namespace MyApplication.UI.WP7.Views
{
    public partial class HomeView 
        : BaseHomeView
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }

    public class BaseHomeView : MvxPhonePage<HomeViewModel>
    {
    }
}