// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using MvvmCross.Core;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    /// <summary>
    /// Would be good to move this into core
    /// </summary>
    public enum ApplicationState
    {
        Unknown,
        Deactivation,   //Active => inactive
        Background,     //Start running in background (either from foreground or inactive state)
        Activation,     //Inative => active
        Foreground,     //Background => Foreground
    }

    /// <summary>
    /// Mark all startup activities with this interface.
    /// A startup activity is an activity that may be launched directly by Android using an intent filter.
    /// </summary>
    public interface IStartupActivity
    {
        /// <summary>
        /// Called after the startup lifecycle has finished. 
        /// The startup activity, whichever it is, can control what it should do in this method.
        /// </summary>
        /// <remarks>
        /// Typical usage 1: 
        /// if a statup activity is the "[MainActivity]" and is a splash screen which don't have a corresponding viewmodel,
        /// close it by calling Finish() in this method. Otherwise leave blank.
        /// 
        /// Typical usage 2: 
        /// if a startup activity is started by an intent filter and is NOT the mainactivity, it won't have a corresponding viewmodel.
        /// close it by calling Finish() in this method. Otherwise leave blank.
        /// 
        /// Note that by default any activity can be started by another app using an intent filter targeting your app. If this happens and the activities are not decorated with IStartupActivity, mvvmcross won't work as expected.
        /// </remarks>
        void FinishActivity();
    }

    /// <summary>
    /// Mark all your root activities with this interface.
    /// </summary>
    /// <remarks>
    /// A root activity shuts down the mvvmcross application to free resources when it is destroyed.
    /// </remarks>
    public interface IShutdownActivity
    {
    }

    public class StartupLifecycleCallback : Java.Lang.Object, Application.IActivityLifecycleCallbacks, IMvxSetupMonitor, IMvxAndroidCurrentTopActivity
    {
        protected int startedActivities;
        protected Activity startupActivity;
        protected bool isStartupActivityDisplayed;
        protected readonly List<Activity> resumedActivities = new List<Activity>(); //all activities after resumed and until paused

        protected Bundle _bundle;
        protected MvxAndroidSetupSingleton setupSingleton;

        protected Dictionary<string,string> pushedData; //Non null when the app is launched from a push notification

        public Activity Activity => resumedActivities.LastOrDefault();

        public virtual void OnActivityCreated(Activity activity, Bundle bundle)
        {
            Console.WriteLine($"OnActivityCreated {activity.GetType().Name}. Is StartupActivity:{(activity is IStartupActivity ? "yes" : "no")}");

            if (activity is IStartupActivity)
            {
                if (Activity != null)
                {
                    //IMvxLog is not available here
                    Console.WriteLine($"OnActivityCreated IMvxAndroidCurrentTopActivity!=null :{Activity.GetType().Name}");
                    return;
                }

                _bundle = bundle;

                //Non null when the app is launched from a push notification
                var extras = activity.Intent?.Extras;
                if (extras != null)
                {
                    var customData = new Dictionary<string, string>();
                    foreach (var key in extras.KeySet())
                    {
                        var o = extras.Get(key);
                        if (o != null)
                            customData.Add(key, o.ToString());
                    }

                    pushedData = customData;
                }
            }
        }

        public virtual void OnActivityDestroyed(Activity activity)
        {
            //Console.WriteLine($"OnActivityDestroyed {activity.GetType().Name} istaskRoot:{activity.IsTaskRoot}");

            if (activity.IsTaskRoot && (activity is IShutdownActivity) && Mvx.IoCProvider != null)
            {
                SetState(ApplicationState.Deactivation);
                var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
                startup.ResetStart();
                setupSingleton?.CancelMonitor(this);
                setupSingleton?.Dispose();
                setupSingleton = null;
            }
        }

        public virtual void OnActivityPaused(Activity activity)
        {
            //Console.WriteLine($"OnActivityPaused {activity.GetType().Name}");
            resumedActivities.Remove(activity);
        }

        public virtual void OnActivityResumed(Activity activity)
        {
            resumedActivities.Add(activity);

            //Console.WriteLine($"OnActivityResumed {activity.GetType().Name} isStartupActivityDisplayed:{isStartupActivityDisplayed} isStartupActivity:{activity is IStartupActivity} startupActivity:{startupActivity?.GetType().Name ?? "-"} top:{Activity?.GetType().Name ?? "-"} startedActivities:{startedActivities}");

            //Note: on restart (tap on app icon), top.Activity = IStartupActivity and startupActivity = null
            var isFirstStart = Activity == null;
            var isOtherStart = Activity is IStartupActivity;
            var isStartingWithAutoClosingActivity = isFirstStart || isOtherStart;
            if (activity is IStartupActivity && isStartingWithAutoClosingActivity)
            {
                //Splash can be resumed twice, for example when appcenter distribute is embedded and the app is first run (as appcenter temporarily switches to the device's browser before the real main activity is displayed)
                //Also, Splash can be recreated by tapping an Android notification while the app is in background. In this last case, startup.IsStarted will be true (and startedActivities will be greater than 1).
                if (!isStartupActivityDisplayed)
                {
                    isStartupActivityDisplayed = true;
                    startupActivity = activity;
                    setupSingleton = setupSingleton ?? MvxAndroidSetupSingleton.EnsureSingletonAvailable(Application.Context);
                    setupSingleton.InitializeAndMonitor(this); //Resume is too late here, the top activity monitor is not up, and will not detect that the current top activity is a splash screen. The fragment presenter will be unable to present the activity.
                }
            }
            else if (isStartupActivityDisplayed)
            {
                //startup activity is always destroyed
                isStartupActivityDisplayed = false;
                startupActivity = null;
                SetState(ApplicationState.Foreground);
            }
            else if (startedActivities == 1)
            {
                SetState(ApplicationState.Foreground);
            }
        }

        public virtual void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            resumedActivities.Remove(activity);
        }

        /// <summary>
        /// Android always starts new activity just before stopping previous one. 
        /// </summary>
        public virtual void OnActivityStarted(Activity activity)
        {
            Console.WriteLine($"OnActivityStarted {activity.GetType().Name}");
            startedActivities++;
        }

        public virtual void OnActivityStopped(Activity activity)
        {
            Console.WriteLine($"OnActivityStopped {activity.GetType().Name} iStartupActivity:{activity is IStartupActivity} isStartupActivityDisplayed:{isStartupActivityDisplayed}");

            if (startedActivities == 1 && (!(activity is IStartupActivity) || isStartupActivityDisplayed))
                SetState(ApplicationState.Background);

            if (startedActivities > 0)
                startedActivities--;
        }

        /// <summary>
        /// Setup triggers this 'initialization completed' event
        /// </summary>
        public virtual Task InitializationComplete()
        {
            var hint = GetAppStartHint(_bundle);
            _bundle = null;
            return RunAppStartAsync(hint);
        }

        protected virtual object GetAppStartHint(Bundle bundle)
        {
            //if (bundle != null)
            //    Console.WriteLine($"AppStartHint: {JsonConvert.SerializeObject(bundle)}");
            return null;
        }

        protected virtual async Task RunAppStartAsync(object hint)
        {
            var ioc = Mvx.IoCProvider;
            var startup = ioc.Resolve<IMvxAppStart>();

            //Non null when the app is launched from a push notification
            if (pushedData != null)
                OnPushedReceived(pushedData);

            if (startup.IsStarted)
            {
                Console.WriteLine("Application activated");

                //When app is reactivated, splash screen activity is displayed.
                //Remove it before triggering Activation state change.
                if (isStartupActivityDisplayed)
                {
                    var activity = Activity;

                    //Fix a rare crash
                    var i = 10;
                    while (activity == null)
                    {
                        await Task.Delay(i++);
                        activity = Activity;
                        if (i == 100)
                        {
                            Console.WriteLine($"Error: startup activity {activity.GetType().Name} can not be destroyed.");
                            return;
                        }
                    }

                    ((IStartupActivity)activity).FinishActivity();
                }

                SetState(ApplicationState.Foreground);

                return;
            }

            //App start
            Console.WriteLine("Application (re) start");
            await startup.StartAsync(hint);
        }

        protected virtual void OnPushedReceived(Dictionary<string, string>  pushedData)
        {
        }

        protected virtual void SetState(ApplicationState applicationState)
        {
            Console.WriteLine($"Application state: {applicationState}");
        }
    }
}
