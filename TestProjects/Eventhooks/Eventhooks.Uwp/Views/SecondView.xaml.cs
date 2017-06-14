using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(SecondViewModel))]
    public sealed partial class SecondView : MvxWindowsPage
    {
        public SecondView()
        {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += EditCustomer_BackRequested;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            SystemNavigationManager.GetForCurrentView().BackRequested -= EditCustomer_BackRequested;
        }

        private void EditCustomer_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.GoBack();
        }

        public SecondViewModel SecondViewModel
        {
            get => (SecondViewModel)ViewModel;
        }
    }
}
