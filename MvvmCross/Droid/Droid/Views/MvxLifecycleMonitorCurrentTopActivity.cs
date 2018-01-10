﻿using System;
using Android.App;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MvvmCross.Droid.Views
{
    // Note that we set Activity = activity in multiple places
    // basically we just want to intercept the activity as early as possible
    // regardless of whether the activity has come from an app switch or a new start or...
    public class MvxLifecycleMonitorCurrentTopActivity : IMvxAndroidCurrentTopActivity
    {
        public MvxLifecycleMonitorCurrentTopActivity(IMvxAndroidActivityLifetimeListener listener)
        {
            listener.ActivityChanged += Listener_ActivityChanged;
        }

        public Activity Activity { get; private set; }

        private void Listener_ActivityChanged(object sender, Views.MvxActivityEventArgs e)
        {
            switch (e.ActivityState)
            {
                case MvxActivityState.OnCreate:
                case MvxActivityState.OnStart:
                case MvxActivityState.OnRestart:
                case MvxActivityState.OnResume:
                case MvxActivityState.OnNewIntent:
                case MvxActivityState.OnSaveInstanceState:

                    Activity = e.Activity;
                    break;

                case MvxActivityState.OnDestroy:

                    if (e.Activity == Activity)
                        Activity = null;
                    break;
            }
        }
    }
}
