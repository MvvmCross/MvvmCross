using System;
using Android.Content;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxTabAttribute : MvxFragmentAttribute
    {
        public MvxTabAttribute(string title, int viewPagerResourceId, int tabLayoutResourceId, Type activityHostViewModelType = null)
        {
            Title = title;
            ActivityHostViewModelType = activityHostViewModelType;
            ViewPagerResourceId = viewPagerResourceId;
            TabLayoutResourceId = tabLayoutResourceId;
        }

        public string Title { get; set; }

        public int ViewPagerResourceId { get; set; }

        public int TabLayoutResourceId { get; set; }
    }
}