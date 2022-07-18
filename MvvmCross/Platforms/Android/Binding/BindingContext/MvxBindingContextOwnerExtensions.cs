// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Platforms.Android.Binding.BindingContext
{
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
