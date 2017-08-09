using System;
using Android.Content;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxTabAttribute : MvxBasePresentationAttribute
    {
        public MvxTabAttribute()
        {

        }

        public MvxTabAttribute(string title, int viewPagerResourceId, int tabLayoutResourceId)
        {
            Title = title;
            ViewPagerResourceId = viewPagerResourceId;
            TabLayoutResourceId = tabLayoutResourceId;
        }

        public MvxTabAttribute(string title, int viewPagerResourceId, int tabLayoutResourceId, Type activityHostViewModelType = null, Type fragmentHostViewType = null) : this(title, viewPagerResourceId, tabLayoutResourceId)
        {
            ActivityHostViewModelType = activityHostViewModelType;
            FragmentHostViewType = fragmentHostViewType;
        }

        public MvxTabAttribute(string title, string viewPagerResourceId, string tabLayoutResourceId, Type activityHostViewModelType = null, Type fragmentHostViewModelType = null)
        {
            var context = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext;

            //TODO: Add shared stuff
        }

        public string Title { get; set; }

        public int ViewPagerResourceId { get; set; }

        public int TabLayoutResourceId { get; set; }

        /// <summary>
        /// Fragment parent activity ViewModel Type. This activity is shown if the current hosting activity viewmodel is different.
        /// </summary>
        public Type ActivityHostViewModelType { get; set; }

        /// <summary>
        /// Fragment parent activity ViewModel Type.
        /// </summary>
        public Type FragmentHostViewType { get; set; }
    }
}