using Cirrious.MvvmCross.WindowsPhone.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.WP7.Views
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