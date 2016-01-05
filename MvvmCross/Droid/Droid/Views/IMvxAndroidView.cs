// IMvxAndroidView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Droid.Views
{
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Droid.Views;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform.Droid.Views;

    public interface IMvxAndroidView
        : IMvxView
        , IMvxLayoutInflaterHolder
        , IMvxStartActivityForResult
        , IMvxBindingContextOwner
    {
    }

    public interface IMvxAndroidView<TViewModel>
        : IMvxAndroidView
        , IMvxView<TViewModel> where TViewModel : class, IMvxViewModel
    {
    }
}