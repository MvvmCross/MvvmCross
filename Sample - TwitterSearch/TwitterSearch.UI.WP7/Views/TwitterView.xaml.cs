using Cirrious.MvvmCross.WindowsPhone.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.WP7.Views
{
    public partial class TwitterView 
        : BaseTwitterView
    {
        public TwitterView()
        {
            InitializeComponent();
        }
    }

    public class BaseTwitterView : MvxPhonePage<TwitterViewModel>
    {
    }
}