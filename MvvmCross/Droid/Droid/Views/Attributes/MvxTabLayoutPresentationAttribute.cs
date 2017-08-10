using System;
using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxTabLayoutPresentationAttribute : MvxViewPagerFragmentPresentationAttribute
    {
        public MvxTabLayoutPresentationAttribute()
        {
        }

        public MvxTabLayoutPresentationAttribute(string title, int viewPagerResourceId, int tabLayoutResourceId, Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null, bool isCacheableFragment = false) : base(title, viewPagerResourceId, activityHostViewModelType, addToBackStack, fragmentHostViewType, isCacheableFragment)
        {
            TabLayoutResourceId = tabLayoutResourceId;
        }

        public MvxTabLayoutPresentationAttribute(string title, string viewPagerResourceName, string tabLayoutResourceName, Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null, bool isCacheableFragment = false) : base(title, viewPagerResourceName, activityHostViewModelType, addToBackStack, fragmentHostViewType, isCacheableFragment)
        {
            var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            TabLayoutResourceId = !string.IsNullOrEmpty(tabLayoutResourceName) ? context.Resources.GetIdentifier(tabLayoutResourceName, "id", context.PackageName) : Android.Resource.Id.Content;
        }

        /// <summary>
        /// The resource used to get the TabLayout from the view
        /// </summary>
        public int TabLayoutResourceId { get; set; }
    }
}