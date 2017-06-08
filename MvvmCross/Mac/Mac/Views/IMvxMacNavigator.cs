
namespace MvvmCross.Mac.Views
{
    using Core.ViewModels;

    public interface IMvxMacNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);
    }
}