using MvvmCross.Core.ViewModels;

namespace MvvmCross.Mac.Views
{
    public interface IMvxMacNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);
    }
}