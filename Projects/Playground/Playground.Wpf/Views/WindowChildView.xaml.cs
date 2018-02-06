using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Wpf.Views.Presenters.Attributes;

namespace Playground.Wpf.Views
{
    public partial class WindowChildView :  IMvxOverridePresentationAttribute
    {
        public WindowChildView()
        {
            InitializeComponent();
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            return new MvxContentPresentationAttribute
            {
                WindowIdentifier = $"{nameof(WindowView)}.ViewModel.ParentNo",
                StackNavigation = false
            };
        }
    }
}
