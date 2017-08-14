// MvxAndroidLifeTimeMonitor.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using MvvmCross.Core.Platform;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Droid.Views
{
    // For lifetime explained, see http://developer.android.com/guide/topics/fundamentals/activities.html
    // Note that we set Activity = activity in multiple places
    // basically we just want to intercept the activity as early as possible
    // regardless of whether the activity has come from an app switch or a new start or...
    public class MvxAndroidLifetimeMonitor
        : MvxLifetimeMonitor, IMvxAndroidActivityLifetimeListener, IMvxAndroidCurrentTopActivity
    {
        private int _createdActivityCount;

        public virtual void OnCreate(Activity activity, Bundle eventArgs)
        {
            _createdActivityCount++;
            if (_createdActivityCount == 1)
            {
                FireLifetimeChange(MvxLifetimeEvent.ActivatedFromDisk);
            }
            Activity = activity;
            FireActivityChange(activity, MvxActivityState.OnCreate, eventArgs);
        }

        public virtual void OnStart(Activity activity)
        {
            Activity = activity;
            FireActivityChange(activity, MvxActivityState.OnStart);
        }

        public virtual void OnRestart(Activity activity)
        {
            Activity = activity;
            FireActivityChange(activity, MvxActivityState.OnRestart);
        }

        public virtual void OnResume(Activity activity)
        {
            Activity = activity;
            FireActivityChange(activity, MvxActivityState.OnResume);
        }

        public virtual void OnPause(Activity activity)
        {
            // ignored
            FireActivityChange(activity, MvxActivityState.OnPause);
        }

        public virtual void OnStop(Activity activity)
        {
            // ignored
            FireActivityChange(activity, MvxActivityState.OnStop);
        }

        public virtual void OnDestroy(Activity activity)
        {
            if (Activity == activity)
                Activity = null;

            _createdActivityCount--;
            if (_createdActivityCount == 0)
            {
                FireLifetimeChange(MvxLifetimeEvent.Closing);
            }
            FireActivityChange(activity, MvxActivityState.OnDestroy);
        }

        public virtual void OnViewNewIntent(Activity activity)
        {
            Activity = activity;
        }

        public void OnSaveInstanceState(Activity activity, Bundle eventArgs)
        {
            Activity = activity;
            FireActivityChange(activity, MvxActivityState.OnSaveInstanceState, eventArgs);
        }

        public Activity Activity { get; private set; }

        protected void FireActivityChange(Activity activity, MvxActivityState state, object extras = null)
        {
            ActivityChanged?.Invoke(this, new MvxActivityEventArgs(activity, state, extras));
        }

        public event EventHandler<MvxActivityEventArgs> ActivityChanged;
    }
}