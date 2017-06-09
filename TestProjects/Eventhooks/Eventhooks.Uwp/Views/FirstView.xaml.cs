using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(FirstViewModel))]
    public sealed partial class FirstView : MvxWindowsPage
    {
        public FirstView()
        {
            InitializeComponent();
        }

        public FirstViewModel FirstViewModel
        {
            get => (FirstViewModel)ViewModel;
        }
    }
}
