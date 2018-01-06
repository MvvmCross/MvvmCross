using MvvmCross.Core.ViewModels;

namespace MvvmCross.Core.Views
{
    public interface IMvxOverridePresentationAttribute
    {
        MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request);
    }
}
