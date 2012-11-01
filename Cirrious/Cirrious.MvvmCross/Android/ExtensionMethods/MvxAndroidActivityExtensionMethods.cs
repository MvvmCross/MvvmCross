#region Copyright
// <copyright file="MvxAndroidActivityExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Android.App;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.ExtensionMethods
{
    public static class MvxAndroidActivityExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnCreate(activity));
            var view = androidView as IMvxView<TViewModel>;
            view.OnViewCreate(() => { return androidView.LoadViewModel(); });
        }

        public static void OnViewNewIntent<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.EnsureSetupInitialized();
            androidView.OnLifetimeEvent((listener, activity) => listener.OnViewNewIntent(activity));
            var view = androidView as IMvxView<TViewModel>;
            view.OnViewNewIntent(() => { return androidView.LoadViewModel(); });
        }

        public static void OnViewDestroy<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnDestroy(activity));
            var view = androidView as IMvxView<TViewModel>;
            view.OnViewDestroy();
        }

        public static void OnViewStart<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStart(activity));
        }

        public static void OnViewRestart<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnRestart(activity));
        }

        public static void OnViewStop<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnStop(activity));
        }

        public static void OnViewResume<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnResume(activity));
        }

        public static void OnViewPause<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            androidView.OnLifetimeEvent((listener, activity) => listener.OnPause(activity));
        }

        private static void OnLifetimeEvent<TViewModel>(this IMvxAndroidView<TViewModel> androidView, Action<IMvxAndroidActivityLifetimeListener, Activity> report)
            where TViewModel : class, IMvxViewModel
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

        private static TViewModel LoadViewModel<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            var activity = androidView.ToActivity();
            if (typeof(TViewModel) == typeof(MvxNullViewModel))
                return new MvxNullViewModel() as TViewModel;

            var translatorService = androidView.GetService<IMvxAndroidViewModelLoader>();
            var viewModel = translatorService.Load(activity.Intent, typeof(TViewModel));

            return (TViewModel)viewModel;
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