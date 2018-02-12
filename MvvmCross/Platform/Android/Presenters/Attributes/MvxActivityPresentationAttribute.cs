// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using MvvmCross.Presenters;

namespace MvvmCross.Platform.Android.Presenters.Attributes
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
