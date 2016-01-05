// IMvxPhoneView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsPhone.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxPhoneView
        : IMvxView
    {
        void ClearBackStack();
    }

    public interface IMvxPhoneView<TViewModel>
        : IMvxPhoneView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}