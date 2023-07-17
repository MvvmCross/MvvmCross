// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views.Base;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Platforms.Android.Views
{
    public static class MvxActivityViewExtensions
    {
        public static void AddEventListeners(this IMvxEventSourceActivity activity)
        {
            if (activity is IMvxAndroidView)
            {
                var adapter = new MvxActivityAdapter(activity);
            }
            if (activity is IMvxBindingContextOwner)
            {
                var bindingAdapter = new MvxBindingActivityAdapter(activity);
            }
        }

        public static void OnViewCreate(this IMvxAndroidView androidView, Bundle bundle)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity, bundle));

            IMvxViewModel cached = null;
            if (Mvx.IoCProvider?.TryResolve<IMvxSingleViewModelCache>(out var cache) == true)
                cached = cache.GetAndClear(bundle);

            var view = (IMvxView)androidView;
            var savedState = GetSavedStateFromBundle(bundle);
            view.OnViewCreate(() => cached ?? androidView.LoadViewModel(savedState));
        }

        private static IMvxBundle GetSavedStateFromBundle(Bundle bundle)
        {
            if (bundle == null)
                return null;

            if (Mvx.IoCProvider?.TryResolve<IMvxSavedStateConverter>(out var converter) != true)
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "No saved state converter available - this is OK if seen during start");
                return null;
            }
            var savedState = converter.Read(bundle);
            return savedState;
        }

        public static void OnViewNewIntent(this IMvxAndroidView androidView)
        {
            MvxLogHost.Default?.Log(LogLevel.Trace, "OnViewNewIntent called - MvvmCross lifecycle won't run automatically in this case");
        }

        public static void OnViewDestroy(this IMvxAndroidView androidView)
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
            var view = androidView as IMvxView;
            view.OnViewDestroy();

            if (Mvx.IoCProvider?.TryResolve<IMvxAppStart>(out var appStart) != true ||
                Mvx.IoCProvider?.TryResolve<IMvxAndroidCurrentTopActivity>(out var topActivity) != true)
            {
                return;
            }

            var currentActivity = topActivity.Activity;
            if (IsActivityTearingDown(currentActivity))
            {
                appStart?.ResetStart();
            }
            else if (currentActivity == null && IsActivityTearingDown(view as Activity))
            {
                appStart?.ResetStart();
            }
        }

        private static bool IsActivityTearingDown(Activity? activity)
        {
            if (activity == null) return false;
            if (activity.IsDestroyed) return true;
            if (activity.IsFinishing) return true;
            return false;
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

        private static void OnLifetimeEvent(
            this IMvxAndroidView androidView,
            Action<IMvxAndroidActivityLifetimeListener, Activity> report)
        {
            var activityTracker = Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>();
            report(activityTracker, androidView.ToActivity());
        }

        public static Activity ToActivity(this IMvxAndroidView androidView)
        {
            var activity = androidView as Activity;
            if (activity == null)
                throw new MvxException("OnViewCreate called from an IMvxView which is not an Android Activity");
            return activity;
        }

        private static IMvxViewModel LoadViewModel(this IMvxAndroidView androidView, IMvxBundle savedState)
        {
            var activity = androidView.ToActivity();

            var viewModelType = androidView.FindAssociatedViewModelTypeOrNull();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(IMvxViewModel))
            {
                MvxLogHost.Default?.Log(LogLevel.Trace, "No ViewModel class specified for {ViewType} in LoadViewModel",
                    androidView.GetType().Name);
            }

            var translatorService = Mvx.IoCProvider.Resolve<IMvxAndroidViewModelLoader>();
            var viewModel = translatorService.Load(activity.Intent, savedState, viewModelType);

            return viewModel;
        }
    }
}
