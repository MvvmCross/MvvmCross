using MvvmCross.Platform.Wpf.Views.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;

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
