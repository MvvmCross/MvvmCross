// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.OS;
using Object = Java.Lang.Object;

namespace MvvmCross.Platforms.Android
{
    public static class MvxJavaObjectExtensions
    {
        public static bool IsNull(this Object @object)
        {
            if (@object == null)
                return true;

            if (@object.Handle == IntPtr.Zero)
                return true;

            return false;
        }

        public static bool IsActivityDead(this Activity activity)
        {
            if (activity.IsNull())
                return true;

            if (activity.IsFinishing)
                return true;

            if (activity.IsDestroyed)
                return true;

            return false;
        }

        public static bool IsActivityAlive(this Activity activity) => !IsActivityDead(activity);
    }
}
