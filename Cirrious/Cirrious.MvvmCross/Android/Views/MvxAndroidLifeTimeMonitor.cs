using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Interfaces.Platform.Lifetime;
using Cirrious.MvvmCross.Platform.Lifetime;

namespace Cirrious.MvvmCross.Android.LifeTime
{
    // For lifetime explained, see http://developer.android.com/guide/topics/fundamentals/activities.html
    // Note that we set Activity = activity in multiple places
    // basically we just want to intercept the activity as early as possible
    // regardless of whether the activity has come from an app switch or a new start or...
    public class MvxAndroidLifetimeMonitor 
        : MvxBaseLifetimeMonitor
        , IMvxAndroidActivityLifetimeListener
        , IMvxAndroidCurrentTopActivity
    {
        private int _createdActivityCount;

        public Activity Activity { get; private set; }

        public void OnCreate(Activity activity)
        {
            _createdActivityCount++;
            if (_createdActivityCount == 1)
            {
                FireLifetimeChange(MvxLifetimeEvent.ActivatedFromDisk);
            }
            Activity = activity;
        }

        public void OnStart(Activity activity)
        {
            Activity = activity;
        }

        public void OnRestart(Activity activity)
        {
            Activity = activity;
        }

        public void OnResume(Activity activity)
        {
            Activity = activity;
        }

        public void OnPause(Activity activity)
        {
            // ignored
        }

        public void OnStop(Activity activity)
        {
            // ignored
        }

        public void OnDestroy(Activity activity)
        {
            if (Activity == activity)
                Activity = null;

            _createdActivityCount--;
            if (_createdActivityCount == 0)
            {
                FireLifetimeChange(MvxLifetimeEvent.Closing);
            }
        }
    }
}
