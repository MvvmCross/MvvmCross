// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxTabLayoutPresentationAttribute : MvxViewPagerFragmentPresentationAttribute
    {
        public MvxTabLayoutPresentationAttribute()
        {
        }

        public MvxTabLayoutPresentationAttribute(string title, int viewPagerResourceId, int tabLayoutResourceId,
            Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null,
            bool isCacheableFragment = false) : base(title, viewPagerResourceId, activityHostViewModelType,
            addToBackStack, fragmentHostViewType, isCacheableFragment)
        {
            TabLayoutResourceId = tabLayoutResourceId;
        }

        public MvxTabLayoutPresentationAttribute(string title, string viewPagerResourceName,
            string tabLayoutResourceName, Type activityHostViewModelType = null, bool addToBackStack = false,
            Type fragmentHostViewType = null, bool isCacheableFragment = false) : base(title, viewPagerResourceName,
            activityHostViewModelType, addToBackStack, fragmentHostViewType, isCacheableFragment)
        {
            var context = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            TabLayoutResourceId = !string.IsNullOrEmpty(tabLayoutResourceName)
                ? context.Resources.GetIdentifier(tabLayoutResourceName, "id", context.PackageName)
                : global::Android.Resource.Id.Content;
        }

        /// <summary>
        ///     The resource used to get the TabLayout from the view
        /// </summary>
        public int TabLayoutResourceId { get; set; }
    }
}
