// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.OS;
using Android.Runtime;

namespace MvvmCross.Platforms.Android.Views
{
#nullable enable
    [Register("mvvmcross.platforms.android.MvxCurrentTopActivity")]
    public class MvxCurrentTopActivity
        : Java.Lang.Object, Application.IActivityLifecycleCallbacks, IMvxAndroidCurrentTopActivity
    {
        private readonly WeakReference<Activity?> _lastSeenActivity = new WeakReference<Activity?>(null);

        public Activity? Activity
        {
            get
            {
                if (_lastSeenActivity?.TryGetTarget(out var activity) ?? false)
                    return activity;
                return null;
            }
        }

        public static bool Initialized { get; set; }

        public void OnActivityCreated(Activity activity, Bundle? savedInstanceState)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityPaused(Activity activity)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityResumed(Activity activity)
        {
            _lastSeenActivity.SetTarget(activity);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            // not interested in this
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            // not interested in this
        }

        public void OnActivityStarted(Activity activity)
        {
            // not interested in this
        }

        public void OnActivityStopped(Activity activity)
        {
            // not interested in this
        }
    }
#nullable restore
}
