﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("mvvmcross.droid.support.v7.appcompat." + nameof(MvxSplashScreenAppCompatActivity))]
    public abstract class MvxSplashScreenAppCompatActivity
        : MvxAppCompatActivity, IMvxSetupMonitor
    {
        private const int NoContent = 0;

        private readonly int _resourceId;

        public new MvxNullViewModel ViewModel
        {
            get { return base.ViewModel as MvxNullViewModel; }
            set { base.ViewModel = value; }
        }

        protected MvxSplashScreenAppCompatActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected virtual void RequestWindowFeatures()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeatures();

            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.InitializeAndMonitor(this);

            base.OnCreate(bundle);

            if (_resourceId != NoContent)
            {
                // Set our view from the "splash" layout resource
                // Be careful to use non-binding inflation
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }

        private bool _isResumed;

        protected override void OnResume()
        {
            base.OnResume();
            _isResumed = true;
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.InitializeAndMonitor(this);
        }

        protected override void OnPause()
        {
            _isResumed = false;
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.CancelMonitor(this);
            base.OnPause();
        }

        public virtual void InitializationComplete()
        {
            if (!_isResumed)
                return;

            TriggerFirstNavigate();
        }

        protected virtual void TriggerFirstNavigate()
        {
            var startup = Mvx.Resolve<IMvxAppStart>();
            if (!startup.IsStarted)
                startup.Start();
        }
    }
}
