// IMvxAutoView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Interfaces
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;

    public interface IMvxAutoView
        : IMvxView
    {
    }

    public interface IMvxAutoView<TViewModel>
        : IMvxAutoView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}