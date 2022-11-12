// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Android.App;
using MvvmCross.Presenters;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
#nullable enable
    public static class MvxAndroidPresentationAttributeExtensions
    {
        public static bool IsFragmentCacheable(this Type fragmentType, Type fragmentActivityParentType)
        {
            if (!fragmentType.HasBasePresentationAttribute())
                return false;

            var fragmentAttributes =
                fragmentType.GetBasePresentationAttributes()
                    .Select(baseAttribute => baseAttribute as MvxFragmentPresentationAttribute)
                    .Where(fragmentAttribute => fragmentAttribute != null);

            var currentAttribute = fragmentAttributes.FirstOrDefault(
                fragmentAttribute => fragmentAttribute != null &&
                fragmentAttribute.ActivityHostViewModelType == fragmentActivityParentType);

            return currentAttribute?.IsCacheableFragment == true;
        }

        public static PopBackStackFlags ToNativePopBackStackFlags(this MvxPopBackStack mvxPopBackStack)
        {
            switch (mvxPopBackStack)
            {
                case MvxPopBackStack.None:
                    return PopBackStackFlags.None;
                case MvxPopBackStack.Inclusive:
                    return PopBackStackFlags.Inclusive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mvxPopBackStack), mvxPopBackStack, $"No matching {nameof(PopBackStackFlags)} enum is defined");
            }
        }
    }
#nullable restore
}
