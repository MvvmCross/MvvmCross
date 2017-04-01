// IMvxModalIosView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace MvvmCross.iOS.Views
{
    public interface IMvxModalIosView : IMvxView
    {
    }

    public interface IMvxModalIosView<TViewModel> : IMvxModalIosView, IMvxView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
    }
}