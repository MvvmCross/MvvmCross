using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using MvvmCross.Core.Views;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxActivityPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxActivityPresentationAttribute()
        {
        }

        public static Bundle DefaultExtras = null;
        /// <summary>
        /// Add extras to the Intent that will be started for this Activity
        /// </summary>
        public Bundle Extras { get; set; } = DefaultExtras;

        public static IDictionary<string, View> DefaultSharedElements = null;
        /// <summary>
        /// SharedElements that will be added to the transition. String may be left empty when using AppCompat
        /// </summary>
        public IDictionary<string, View> SharedElements { get; set; } = DefaultSharedElements;
    }
}