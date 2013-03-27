// MvxActivityViewExtensions.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.App;
using Android.OS;
using Cirrious.CrossCore.Droid.Views;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Droid.Views
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
            if (activity is IMvxChildViewModelOwner)
            {
                var childOwnerAdapter = new MvxChildViewModelOwnerAdapter(activity);
            }
        }

        public static void OnViewCreate(this IMvxAndroidView androidView, Bundle bundle)
        {
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity));

            var cache = Mvx.Resolve<IMvxSingleViewModelCache>();
            var cached = cache.GetAndClear(bundle);

            var view = androidView as IMvxView;
            var savedState = GetSavedStateFromBundle(bundle);
            view.OnViewCreate(() => { return cached ?? androidView.LoadViewModel(savedState); });
        }

        private static IMvxBundle GetSavedStateFromBundle(Bundle bundle)
        {
            if (bundle == null)
                return new MvxBundle();

            IMvxSavedStateConverter converter; 
            if (!Mvx.TryResolve<IMvxSavedStateConverter>(out converter))
            {
                MvxTrace.Trace("No saved state converter available - this is OK if seen during start");
                return new MvxBundle();
            }
            var savedState = converter.Read(bundle);
            return savedState;
        }

        public static void OnViewNewIntent(this IMvxAndroidView androidView)
        {
            throw new MvxException("Sorry - we don't currently support OnNewIntent in MvvmCross-Android");
            /*
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnViewNewIntent(activity));
            
            var view = androidView as IMvxView;
            MvxTrace.Warning(
                           "OnViewNewIntent isn't well understood or tested inside MvvmCross - it's not really a cross-platform concept.");
            view.OnViewNewIntent(() => { return androidView.LoadViewModel(null); });
             */
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
            var activityTracker = Mvx.Resolve<IMvxAndroidActivityLifetimeListener>();
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
                || viewModelType == typeof (IMvxViewModel))
            {
                MvxTrace.Warning("No ViewModel class specified for {0} - returning null from LoadViewModel",
                               androidView.GetType().Name);
                return null;
            }

            var translatorService = Mvx.Resolve<IMvxAndroidViewModelLoader>();
            var viewModel = translatorService.Load(activity.Intent, savedState, viewModelType);

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