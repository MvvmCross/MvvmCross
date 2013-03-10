using Cirrious.MvvmCross.WindowsPhone.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.WP7.Views
{
    public partial class TwitterView 
        : MvxPhonePage
    {
        public TwitterView()
        {
            InitializeComponent();
        }

        public new TwitterViewModel ViewModel
        {
            get { return (TwitterViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}