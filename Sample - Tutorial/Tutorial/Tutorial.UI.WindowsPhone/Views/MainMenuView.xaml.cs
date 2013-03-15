using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels;

namespace Tutorial.UI.WindowsPhone.Views
{
    public partial class MainMenuView : MvxPhonePage
    {
        public new MainMenuViewModel ViewModel
        {
            get { return (MainMenuViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MainMenuView()
        {
            InitializeComponent();
        }
    }
}