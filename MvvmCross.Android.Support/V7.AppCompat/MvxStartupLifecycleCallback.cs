// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;

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
    /// Mark all statup activities with this interface.
    /// A startup activity may be launched directly by Android using an intent filter.
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
    /// You MUST mark Activities that can shut downs the app with IShutdownActivity.
    /// Otherwise opening the app again by tapping its icon will display nothing, and tapping the icon again will only display the splash screen.
    /// </summary>
    /// <remarks>
    /// An activity can shut downs the app if it is the last activity that can be displayed by the app.
    /// ie: if the user taps "back" while this activity is displayed, the app is shut down.
    /// </remarks>
    public interface IShutdownActivity
    {
    }

    public class MvxStartupLifecycleCallback : Java.Lang.Object, Application.IActivityLifecycleCallbacks, IMvxSetupMonitor, IMvxAndroidCurrentTopActivity
    {
        protected IMvxAndroidActivityLifetimeListener listener;
        protected readonly List<string> startedActivityNames = new List<string>();

        protected int startedActivities;
        protected Activity startupActivity;
        protected bool isStartupActivityDisplayed;
        protected readonly List<Activity> resumedActivities = new List<Activity>(); //all activities after resumed and until paused

        protected Bundle _bundle;
        protected MvxAndroidSetupSingleton setupSingleton;

        protected Dictionary<string,string> pushedData; //Non null when the app is launched from a push notification

        public Activity Activity => resumedActivities.LastOrDefault();

        public MvxStartupLifecycleCallback()
        {
        }

        [Android.Runtime.Preserve]
        protected MvxStartupLifecycleCallback(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }

        public virtual void OnActivityCreated(Activity activity, Bundle bundle)
        {
            Console.WriteLine($"(SLC) OnActivityCreated {activity.GetType().Name}. Is StartupActivity:{(activity is IStartupActivity ? "yes" : "no")}");

            if (activity is IStartupActivity)
            {
                if (Activity == null)
                {
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
                else
                {
                    Console.WriteLine($"(SLC) OnActivityCreated IStartupActivity!=null :{Activity.GetType().Name}");
                }
            }

            if (activity is IMvxAndroidView mvxAndroidActivity)
                mvxAndroidActivity.OnViewCreate(bundle);

            if (activity is IMvxView mvxActivity)
                mvxActivity.ViewModel?.ViewCreated();

            listener?.OnCreate(activity, bundle);
        }

        public virtual void OnActivityDestroyed(Activity activity)
        {
            //Console.WriteLine($"(SLC) OnActivityDestroyed {activity.GetType().Name} istaskRoot:{activity.IsTaskRoot} isShutdownActivity:{activity is IShutdownActivity} isFinishing:{activity.IsFinishing}");
            listener?.OnDestroy(activity);

            if (activity is IMvxView mvxActivity)
                DestroyActivity(mvxActivity);

            if (activity.IsTaskRoot && activity is IShutdownActivity && Mvx.IoCProvider!=null && activity.IsFinishing)
            {
                Console.WriteLine("(SLC) Application deactivated");
                SetState(ApplicationState.Deactivation);
                if(setupSingleton != null)
                    Console.WriteLine("(SLC) setupSingleton monitor cancelled");
                var startup = Mvx.IoCProvider.Resolve<IMvxAppStart>();
                startup.ResetStart();
                setupSingleton?.CancelMonitor(this);
                setupSingleton?.Dispose();
                setupSingleton = null;
            }
        }

        public virtual void OnActivityPaused(Activity activity)
        {
            //Console.WriteLine($"(SLC) OnActivityPaused {activity.GetType().Name}");
            resumedActivities.Remove(activity);
            listener?.OnPause(activity);

            if (activity is IMvxView mvxActivity)
                mvxActivity.ViewModel?.ViewDisappearing();
        }

        public virtual void OnActivityResumed(Activity activity)
        {
            resumedActivities.Add(activity);

            //Console.WriteLine($"(SLC) OnActivityResumed {activity.GetType().Name} isStartupActivityDisplayed:{isStartupActivityDisplayed} isStartupActivity:{activity is IStartupActivity} startupActivity:{startupActivity?.GetType().Name ?? "-"} top:{Activity?.GetType().Name??"-"} startedActivities:{startedActivities} {startedActivityNames.Aggregate(new StringBuilder(), (sb,s) => sb.Append(s).Append(','))}");

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

            if (activity is IMvxView mvxActivity)
                mvxActivity.ViewModel?.ViewAppeared();

            listener?.OnResume(activity);
        }

        public virtual void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            //Console.WriteLine($"(SLC) OnActivitySaveInstanceState {activity.GetType().Name}");
            resumedActivities.Remove(activity);
            listener?.OnSaveInstanceState(activity, outState);
        }

        /// <summary>
        /// Android always starts new activity just before stopping previous one. 
        /// </summary>
        public virtual void OnActivityStarted(Activity activity)
        {
            Console.WriteLine($"(SLC) OnActivityStarted {activity.GetType().Name}");
            startedActivities++;
            startedActivityNames.Add(activity.GetType().Name);

            if (activity is IMvxView mvxActivity)
                mvxActivity.ViewModel?.ViewAppearing();

            listener?.OnStart(activity);
        }

        public virtual void OnActivityStopped(Activity activity)
        {
            Console.WriteLine($"(SLC) OnActivityStopped {activity.GetType().Name} iStartupActivity:{activity is IStartupActivity} isStartupActivityDisplayed:{isStartupActivityDisplayed} startedActivities:{startedActivities}");

            if (startedActivities == 1 && (!(activity is IStartupActivity) || isStartupActivityDisplayed))
                SetState(ApplicationState.Background);

            if (startedActivities > 0)
            {
                startedActivities--;
                startedActivityNames.RemoveAt(startedActivities);
            }

            if (activity is IMvxView mvxActivity)
                mvxActivity.ViewModel?.ViewDisappeared();

            listener?.OnStop(activity);
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
            //if(bundle != null)
            //    Console.WriteLine($"(SLC) AppStartHint: {JsonConvert.SerializeObject(bundle)}");
            return null;
        }

        protected virtual async Task RunAppStartAsync(object hint)
        {
            var ioc = Mvx.IoCProvider;
            ioc.CallbackWhenRegistered<IMvxAndroidActivityLifetimeListener>(() => listener = Mvx.IoCProvider.GetSingleton<IMvxAndroidActivityLifetimeListener>());

            var startup = ioc.Resolve<IMvxAppStart>();
            //Console.WriteLine($"(SLC) RunAppStartAsync isStarted:{startup.IsStarted} isStartupActivityDisplayed:{isStartupActivityDisplayed} startupActivity:{startupActivity?.GetType().Name}");

            //Non null when the app is launched from a push notification
            if (pushedData != null)
                OnPushedReceived(pushedData);

            if (startup.IsStarted)
            {
                //App has been reactivated from background (via a notification or a tap on the app's icon)
                Console.WriteLine("(SLC) Application activated");

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
                            Console.WriteLine($"(SLC) Error: startup activity {activity.GetType().Name} can not be destroyed.");
                            return;
                        }
                    }

                    ((IStartupActivity)activity).FinishActivity();
                }

                SetState(ApplicationState.Foreground);

                return;
            }

            //App 1st start
            Console.WriteLine("(SLC) Application (re) start");

            await startup.StartAsync(hint);
        }

        protected virtual void OnPushedReceived(Dictionary<string, string>  pushedData)
        {
        }

        protected virtual void SetState(ApplicationState applicationState)
        {
            Console.WriteLine($"Application state: {applicationState}");
        }

        protected virtual void DestroyActivity(IMvxView mvxActivity)
        {
            //(mvxActivity.ViewModel as IBackViewModel)?.NonCancellableBackCommand.Execute(null);
            mvxActivity.ViewModel?.ViewDestroy();
        }
    }
}
