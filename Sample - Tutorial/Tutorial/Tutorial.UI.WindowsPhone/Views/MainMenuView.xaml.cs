using System.Collections.Specialized;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Tutorial.Core.ViewModels;

namespace Tutorial.UI.WindowsPhone.Views
{
    public partial class MainMenuView : BaseMainMenuView
    {
        public MainMenuView()
        {
            InitializeComponent();
        }
    }

    public class BaseMainMenuView : MvxPhonePage<MainMenuViewModel> { }
}