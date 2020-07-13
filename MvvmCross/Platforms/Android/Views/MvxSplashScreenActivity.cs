// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.ViewModels;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;

namespace MvvmCross.Platforms.Android.Views
{
    [Register("mvvmcross.platforms.android.views.MvxSplashScreenActivity")]
    public abstract class MvxSplashScreenActivity
        : Activity
    {
        protected const int NoContent = 0;

        private readonly int _resourceId;

        private Bundle? _bundle;

        protected MvxSplashScreenActivity(int resourceId = NoContent)
        {
            RegisterSetup();
            _resourceId = resourceId;
        }

        protected MvxSplashScreenActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected virtual void RequestWindowFeatures()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeatures();

            _bundle = bundle;

            base.OnCreate(bundle);

            if (_resourceId != NoContent)
            {
                // Set our view from the "splash" layout resource
                // Be careful to use non-binding inflation
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }

        protected override void OnResume()
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            Task.Run(async () => await Initialize(setup).ConfigureAwait(false));
            base.OnResume();
        }

        private async ValueTask Initialize(MvxAndroidSetupSingleton setup)
        {
            await setup.EnsureInitialized().ConfigureAwait(false);

            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup) && !startup.IsStarted)
            {
                await startup.Start(GetAppStartHint(null)).ConfigureAwait(false);
            }

            base.Finish();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected virtual object? GetAppStartHint(object? hint = null)
        {
            return hint;
        }

        protected virtual void RegisterSetup()
        {
        }
    }

    public abstract class MvxSplashScreenActivity<TMvxAndroidSetup, TApplication> : MvxSplashScreenActivity
            where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
            where TApplication : class, IMvxApplication, new()
    {
        protected MvxSplashScreenActivity(int resourceId = NoContent) : base(resourceId)
        {
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
