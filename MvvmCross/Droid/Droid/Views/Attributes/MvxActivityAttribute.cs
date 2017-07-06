// MvxUnconventionalAttribute.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.OS;

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
    }
}