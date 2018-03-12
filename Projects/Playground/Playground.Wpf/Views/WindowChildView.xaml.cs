using MvvmCross.Platform.Wpf.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;

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
