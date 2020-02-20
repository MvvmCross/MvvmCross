// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

#pragma warning disable SA1402 // FileMayOnlyContainASingleType

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxAppCompatApplication : MvxAndroidApplication, IMvxActivityLifecycleCallbacksProvider
    {
        private IActivityLifecycleCallbacks activityLifecycle;

        /// <summary>
        /// Using the top activity discovered by MvxStartupLifecycleCallback works in more situations than using the old MvxApplicationCallbacksCurrentTopActivity.
        /// But the IoCProvider is not yet available. So make it discoverable by MvxAndroidSetup which will register it.
        /// As CreateActivityLifecycleObserver can be overriden, the resulting IActivityLifecycleCallbacks may not implement IMvxAndroidCurrentTopActivity. Make sure this is handled.
        /// </summary>
        public IMvxAndroidCurrentTopActivity AndroidCurrentTopActivityFinder => activityLifecycle as IMvxAndroidCurrentTopActivity;

        public MvxAppCompatApplication()
        {
        }

        protected MvxAppCompatApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            activityLifecycle = CreateActivityLifecycleObserver();
            RegisterActivityLifecycleCallbacks(activityLifecycle);
        }

        protected virtual IActivityLifecycleCallbacks CreateActivityLifecycleObserver() => new MvxStartupLifecycleCallback();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnregisterActivityLifecycleCallbacks(activityLifecycle);
                activityLifecycle.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    public abstract class MvxAppCompatApplication<TMvxAndroidSetup, TApplication> : MvxAppCompatApplication
          where TMvxAndroidSetup : MvxAppCompatSetup<TApplication>, new()
          where TApplication : class, IMvxApplication, new()
    {
        public MvxAppCompatApplication()
        {
        }

        protected MvxAppCompatApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
