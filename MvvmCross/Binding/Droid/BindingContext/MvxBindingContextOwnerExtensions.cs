// MvxBindingContextOwnerExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.BindingContext
{
    using Android.Views;

    using MvvmCross.Binding.BindingContext;

    public static class MvxBindingContextOwnerExtensions
    {
        public static View BindingInflate(this IMvxBindingContextOwner owner, int resourceId, ViewGroup viewGroup)
        {
            var context = (IMvxAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup);
        }

        public static View BindingInflate(this IMvxBindingContextOwner owner, int resourceId, ViewGroup viewGroup, bool attachToParent)
        {
            var context = (IMvxAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup, attachToParent);
        }
    }
}