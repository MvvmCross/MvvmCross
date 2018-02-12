// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using Android.App;
using Android.OS;

namespace MvvmCross.Platform.Android.Views
{
    public class MvxApplicationCallbacksCurrentTopActivity : Java.Lang.Object, Application.IActivityLifecycleCallbacks, IMvxAndroidCurrentTopActivity
    {
        private ConcurrentDictionary<string, ActivityInfo> _Activities = new ConcurrentDictionary<string, ActivityInfo>();

        public Activity Activity => GetCurrentActivity();

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            var activityName = GetActivityName(activity);
            _Activities.GetOrAdd(activityName, new ActivityInfo { Activity = activity, IsCurrent = true });
        }

        public void OnActivityDestroyed(Activity activity)
        {
            var activityName = GetActivityName(activity);
            _Activities.TryRemove(activityName, out ActivityInfo removed);
        }

        public void OnActivityPaused(Activity activity)
        {
            ActivityInfo toAdd = new ActivityInfo { Activity = activity, IsCurrent = false };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, toAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = false;
                return existing;
            });
        }

        public void OnActivityResumed(Activity activity)
        {
            ActivityInfo toAdd = new ActivityInfo { Activity = activity, IsCurrent = true };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, toAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = true;
                return existing;
            });
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            ActivityInfo toAdd = new ActivityInfo { Activity = activity, IsCurrent = true };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, toAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = true;
                return existing;
            });
        }

        public void OnActivityStopped(Activity activity)
        {
            ActivityInfo toAdd = new ActivityInfo { Activity = activity, IsCurrent = false };
            var activityName = GetActivityName(activity);
            _Activities.AddOrUpdate(activityName, toAdd, (key, existing) =>
            {
                existing.Activity = activity;
                existing.IsCurrent = false;
                return existing;
            });
        }

        private Activity GetCurrentActivity()
        {
            if (_Activities.Count > 0)
            {
                var e = _Activities.GetEnumerator();
                while (e.MoveNext())
                {
                    var current = e.Current;
                    if (current.Value.IsCurrent)
                    {
                        return current.Value.Activity;
                    }
                }
            }

            return null;
        }

        protected string GetActivityName(Activity activity) => activity.Class.SimpleName;

        /// <summary>
        /// Used to store additional info along with an activity.
        /// </summary>
        private class ActivityInfo
        {
            public bool IsCurrent { get; set; }
            public Activity Activity { get; set; }
        }
    }
}
