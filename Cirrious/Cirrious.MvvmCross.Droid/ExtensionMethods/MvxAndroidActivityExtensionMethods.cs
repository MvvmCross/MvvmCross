// MvxAndroidActivityExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Platform.Diagnostics;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.Droid.Interfaces;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.ExtensionMethods
{
    public static class MvxAndroidActivityExtensionMethods
    {
        public static void OnViewCreate(this IMvxAndroidView androidView)
        {
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity));
            var view = androidView as IMvxView;
            view.OnViewCreate(() => { return androidView.LoadViewModel(); });
        }

        public static void OnViewNewIntent(this IMvxAndroidView androidView)
        {
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnViewNewIntent(activity));
            var view = androidView as IMvxView;
            view.OnViewNewIntent(() => { return androidView.LoadViewModel(); });
        }

        public static void OnViewDestroy(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
            var view = androidView as IMvxView;
            view.OnViewDestroy();
        }

        public static void OnViewStart(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStart(activity));
        }

        public static void OnViewRestart(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnRestart(activity));
        }

        public static void OnViewStop(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStop(activity));
        }

        public static void OnViewResume(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnResume(activity));
        }

        public static void OnViewPause(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnPause(activity));
        }

        private static void OnLifetimeEvent(this IMvxAndroidView androidView,
                                                        Action<IMvxAndroidActivityLifetimeListener, Activity> report)
        {
            var activityTracker = androidView.GetService<IMvxAndroidActivityLifetimeListener>();
            report(activityTracker, androidView.ToActivity());
        }

        public static Activity ToActivity(this IMvxAndroidView androidView)
        {
            var activity = androidView as Activity;
            if (activity == null)
                throw new MvxException("OnViewCreate called from an IMvxView which is not an Android Activity");
            return activity;
        }

        private static IMvxViewModel LoadViewModel(this IMvxAndroidView androidView)
        {
            var activity = androidView.ToActivity();

            var viewModelType = androidView.ReflectionGetViewModelType();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == typeof (IMvxViewModel))
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "No ViewModel class specified for {0} - returning null from LoadViewModel", androidView.GetType().Name);
                return null;
            }

            var translatorService = androidView.GetService<IMvxAndroidViewModelLoader>();
            var viewModel = translatorService.Load(activity.Intent, viewModelType);

            return viewModel;
        }

        private static void EnsureSetupInitialized(this IMvxAndroidView androidView)
        {
            if (androidView is IMvxAndroidSplashScreenActivity)
            {
                // splash screen views manage their own setup initialization
                return;
            }

            var activity = androidView.ToActivity();
            var setup = MvxAndroidSetupSingleton.GetOrCreateSetup(activity.ApplicationContext);
            setup.EnsureInitialized(androidView.GetType());
        }
    }
}