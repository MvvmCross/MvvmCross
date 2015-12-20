
namespace MvvmCross.Mac.Views
{
    using global::MvvmCross.Core.ViewModels;

    public interface IMvxMacNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void ChangePresentation(MvxPresentationHint hint);
    }
}