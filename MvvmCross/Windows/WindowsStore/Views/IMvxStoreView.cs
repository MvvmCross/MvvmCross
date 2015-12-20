// IMvxStoreView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.WindowsStore.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxStoreView
        : IMvxView
    {
        void ClearBackStack();
    }

    public interface IMvxStoreView<TViewModel>
        : IMvxStoreView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}