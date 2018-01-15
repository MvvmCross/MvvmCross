﻿using System;
using Android;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxViewPagerFragmentPresentationAttribute : MvxFragmentPresentationAttribute
    {
        public MvxViewPagerFragmentPresentationAttribute()
        {
        }

        public MvxViewPagerFragmentPresentationAttribute(string title, int viewPagerResourceId,
            Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null,
            bool isCacheableFragment = false) : base(activityHostViewModelType, int.MinValue, addToBackStack,
            int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue, fragmentHostViewType,
            isCacheableFragment)
        {
            Title = title;
            ViewPagerResourceId = viewPagerResourceId;
        }

        public MvxViewPagerFragmentPresentationAttribute(string title, string viewPagerResourceName,
            Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null,
            bool isCacheableFragment = false) : base(activityHostViewModelType, null, addToBackStack, null, null, null,
            null, null, fragmentHostViewType, isCacheableFragment)
        {
            var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            Title = title;
            ViewPagerResourceId = !string.IsNullOrEmpty(viewPagerResourceName)
                ? context.Resources.GetIdentifier(viewPagerResourceName, "id", context.PackageName)
                : Android.Resource.Id.Content;
        }

        /// <summary>
        ///     The title for the ViewPager. Also used as Title for TabLayout when using MvxTabLayoutPresentationAttribute
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The resource used to get the ViewPager from the view
        /// </summary>
        public int ViewPagerResourceId { get; set; }
    }
}
