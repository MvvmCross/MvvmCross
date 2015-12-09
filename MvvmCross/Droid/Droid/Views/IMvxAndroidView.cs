// IMvxAndroidView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Droid.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
{
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