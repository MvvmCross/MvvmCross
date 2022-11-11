// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.OS;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Platforms.Android.Presenters.Attributes
{
#nullable enable
    [AttributeUsage(AttributeTargets.Class)]
    public class MvxActivityPresentationAttribute : MvxBasePresentationAttribute
    {
        public MvxActivityPresentationAttribute()
        {
        }

        public static Bundle? DefaultExtras { get; }

        /// <summary>
        /// Add extras to the Intent that will be started for this Activity
        /// </summary>
        public Bundle? Extras { get; set; } = DefaultExtras;
    }
#nullable restore
}
