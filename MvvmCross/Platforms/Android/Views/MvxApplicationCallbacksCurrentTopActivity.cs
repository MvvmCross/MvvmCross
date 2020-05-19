﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using Android.App;
using Android.OS;

namespace MvvmCross.Platforms.Android.Views
{
    public class MvxApplicationCallbacksCurrentTopActivity 
        : Java.Lang.Object, Application.IActivityLifecycleCallbacks, IMvxAndroidCurrentTopActivity
    {
        protected ConcurrentDictionary<string, ActivityInfo> Activities { get; }
            = new ConcurrentDictionary<string, ActivityInfo>();

        public Activity Activity => GetCurrentActivity();
        private Activity _lastKnownActivity;

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            UpdateActivityListItem(activity, true);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            var activityName = GetActivityName(activity);
            Activities.TryRemove(activityName, out var removed);
        }

        public void OnActivityPaused(Activity activity)
        {
            UpdateActivityListItem(activity, false);
        }

        public void OnActivityResumed(Activity activity)
        {
            UpdateActivityListItem(activity, true);

            _lastKnownActivity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            UpdateActivityListItem(activity, true);

            _lastKnownActivity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
            UpdateActivityListItem(activity, false);
        }

        protected virtual void UpdateActivityListItem(Activity activity, bool isCurrent)
        {
            var toAdd = new ActivityInfo { Activity = activity, IsCurrent = isCurrent };
            var activityName = GetActivityName(activity);
            Activities.AddOrUpdate(activityName, toAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = isCurrent;
                return existing;
            });
        }

        protected virtual Activity GetCurrentActivity()
        {
            if (Activities.Count > 0)
            {
                Activity currentActivity = null;

                using (var e = Activities.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        var (_, value) = e.Current;
                        if (value.IsCurrent)
                        {
                            currentActivity = value.Activity;
                            break;
                        }
                    }
                }

                if (currentActivity == null)
                {
                    if (!_lastKnownActivity.IsActivityDead() && 
                        _lastKnownActivity.GetType() != typeof(MvxSplashScreenActivity) &&
                        _lastKnownActivity.GetType() != typeof(MvxSplashScreenActivity<,>))
                        return _lastKnownActivity;
                }

                return currentActivity;
            }

            return null;
        }

        protected virtual string GetActivityName(Activity activity) => 
            $"{activity.Class.SimpleName}_{activity.Handle.ToString()}";

        /// <summary>
        /// Used to store additional info along with an activity.
        /// </summary>
        protected class ActivityInfo
        {
            public bool IsCurrent { get; set; }
            public Activity Activity { get; set; }
        }
    }
}
