// IMvxMacView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.Mac.Views
{
    public interface IMvxMacView
        : IMvxView
            , IMvxCanCreateMacView
            , IMvxBindingContextOwner
    {
        MvxViewModelRequest Request { get; set; }
    }

    public interface IMvxMacView<TViewModel>
        : IMvxMacView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}