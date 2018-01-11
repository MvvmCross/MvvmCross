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

namespace MvvmCross.Droid.Views
{
    // For lifetime explained, see http://developer.android.com/guide/topics/fundamentals/activities.html
    public class MvxAndroidLifetimeMonitor
        : MvxLifetimeMonitor, IMvxAndroidActivityLifetimeListener
    {
        private int _createdActivityCount;

        public virtual void OnCreate(Activity activity, Bundle eventArgs)
        {
            _createdActivityCount++;
            if (_createdActivityCount == 1)
            {
                FireLifetimeChange(MvxLifetimeEvent.ActivatedFromDisk);
            }
            FireActivityChange(activity, MvxActivityState.OnCreate, eventArgs);
        }

        public virtual void OnStart(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnStart);
        }

        public virtual void OnRestart(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnRestart);
        }

        public virtual void OnResume(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnResume);
        }

        public virtual void OnPause(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnPause);
        }

        public virtual void OnStop(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnStop);
        }

        public virtual void OnDestroy(Activity activity)
        {
            _createdActivityCount--;
            if (_createdActivityCount == 0)
            {
                FireLifetimeChange(MvxLifetimeEvent.Closing);
            }
            FireActivityChange(activity, MvxActivityState.OnDestroy);
        }

        public virtual void OnViewNewIntent(Activity activity)
        {
            FireActivityChange(activity, MvxActivityState.OnNewIntent);
        }

        public virtual void OnSaveInstanceState(Activity activity, Bundle eventArgs)
        {
            FireActivityChange(activity, MvxActivityState.OnSaveInstanceState, eventArgs);
        }

        protected void FireActivityChange(Activity activity, MvxActivityState state, object extras = null)
        {
            ActivityChanged?.Invoke(this, new MvxActivityEventArgs(activity, state, extras));
        }

        public event EventHandler<MvxActivityEventArgs> ActivityChanged;
    }
}