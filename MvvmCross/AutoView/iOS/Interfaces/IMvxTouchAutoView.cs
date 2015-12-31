// IMvxTouchAutoView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Interfaces
{
    using MvvmCross.AutoView.Interfaces;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.iOS.Views;

    public interface IMvxTouchAutoView
        : IMvxTouchView
          , IMvxAutoView
    {
    }

    public interface IMvxTouchAutoView<TViewModel>
        : IMvxTouchAutoView
          , IMvxTouchView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        new TViewModel ViewModel { get; set; }
    }
}