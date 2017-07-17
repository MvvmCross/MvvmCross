using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;
using Windows.UI.Core;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(FirstViewModel))]
    public sealed partial class FirstView : MvxWindowsPage
    {
        public FirstView()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public FirstViewModel FirstViewModel
        {
            get => (FirstViewModel)ViewModel;
        }
    }
}
