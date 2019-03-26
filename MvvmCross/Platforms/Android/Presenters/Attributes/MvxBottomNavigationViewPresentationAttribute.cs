using System;
using MvvmCross;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MvxBottomNavigationViewPresentationAttribute : MvxFragmentPresentationAttribute
    {
        public MvxBottomNavigationViewPresentationAttribute()
        {
        }

        public MvxBottomNavigationViewPresentationAttribute(int bottomNavigationResourceId, string title, int drawableItemId = int.MinValue,
            Type activityHostViewModelType = null, int fragmentContentId = int.MinValue, bool addToBackStack = false, Type fragmentHostViewType = null,
            bool isCacheableFragment = false, string tag = null)
            : base(activityHostViewModelType, fragmentContentId, addToBackStack,
                  int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue, fragmentHostViewType,
                  isCacheableFragment, tag)
        {
            BottomNavigationResourceId = bottomNavigationResourceId;
            DrawableItemId = drawableItemId;
            Title = title;
        }

        public MvxBottomNavigationViewPresentationAttribute(string bottomNavigationResourceName, string title, int drawableItemId = int.MinValue,
            Type activityHostViewModelType = null, bool addToBackStack = false, Type fragmentHostViewType = null,
            bool isCacheableFragment = false, string tag = null)
            : base(activityHostViewModelType, null, addToBackStack, null, null, null,
                null, null, fragmentHostViewType, isCacheableFragment, tag)
        {
            var context = Mvx.IoCProvider.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            BottomNavigationResourceId = !string.IsNullOrEmpty(bottomNavigationResourceName)
                ? context.Resources.GetIdentifier(bottomNavigationResourceName, "id", context.PackageName)
                : global::Android.Resource.Id.Content;
            DrawableItemId = drawableItemId;
            Title = title;
        }

        /// <summary>
        ///     The resource used to get the BottomNavigation from the view
        /// </summary>
        public int BottomNavigationResourceId { get; set; }

        /// <summary>
        ///     The resource used to get the menu item from the view
        /// </summary>
        public int DrawableItemId { get; set; }

        /// <summary>
        ///     The title for the bottom navigation menu item
        /// </summary>
        public string Title { get; set; }
    }
}
