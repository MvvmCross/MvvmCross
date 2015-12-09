// IMvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.BindingContext
{
    using Android.Views;

    using MvvmCross.Binding.Droid.Views;

    public interface IMvxAndroidBindingContext
        : IMvxBindingContext
    {
        IMvxLayoutInflaterHolder LayoutInflaterHolder { get; set; }

        View BindingInflate(int resourceId, ViewGroup viewGroup);

        View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToParent);
    }
}