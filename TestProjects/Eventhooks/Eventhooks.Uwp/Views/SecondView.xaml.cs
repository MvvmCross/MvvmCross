using Eventhooks.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Uwp.Views;

namespace Eventhooks.Uwp.Views
{
    [MvxViewFor(typeof(SecondViewModel))]
    public sealed partial class SecondView : MvxWindowsPage
    {
        public SecondView()
        {
            InitializeComponent();
        }

        public SecondViewModel SecondViewModel
        {
            get => (SecondViewModel)ViewModel;
        }
    }
}
