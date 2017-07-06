using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;

namespace MvvmCross.Droid.Views.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MvxActivityAttribute : MvxBasePresentationAttribute
    {
        public MvxActivityAttribute()
        {
        }

        /// <summary>
        /// Add extras to the Intent that will be started for this Activity
        /// </summary>
        public Bundle Extras { get; set; }

        /// <summary>
        /// SharedElements that will be added to the transition. String may be left empty when using AppCompat
        /// </summary>
        public IDictionary<string, View> SharedElements { get; set; }
    }
}