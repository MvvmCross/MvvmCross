// IMvxAndroidBindingContext.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid.BindingContext
{
    public interface IMvxAndroidBindingContext
        : IMvxBindingContext
    {
        IMvxLayoutInflaterHolder LayoutInflaterHolder { get; set; }
        View BindingInflate(int resourceId, ViewGroup viewGroup);
        View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToParent);
    }
}