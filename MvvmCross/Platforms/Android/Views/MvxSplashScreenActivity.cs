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
        : Activity, IMvxSetupMonitor
    {
        protected const int NoContent = 0;

        private readonly int _resourceId;

        private Bundle? _bundle;

        //public new MvxNullViewModel? ViewModel
        //{
        //    get { return base.ViewModel as MvxNullViewModel; }
        //    set { base.ViewModel = value; }
        //}

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

            //var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);

            //await setup.InitializeAndMonitor(this).ConfigureAwait(false);
            //new Handler().PostDelayed(async() =>
            //{
            //    await setup.InitializeAndMonitor(this).ConfigureAwait(false);
            //}, 10000);

            //Task.Run(async () => await setup.InitializeAndMonitor(this).ConfigureAwait(false)).Wait();

            base.OnCreate(bundle);

            if (_resourceId != NoContent)
            {
                // Set our view from the "splash" layout resource
                // Be careful to use non-binding inflation
                var content = LayoutInflater.Inflate(_resourceId, null);
                SetContentView(content);
            }
        }

        //protected override void OnStart()
        //{
        //    var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);

        //    Task.Run(async() => await setup.InitializeAndMonitor(this).ConfigureAwait(false)).Wait();

        //    //new Handler().PostDelayed(async () =>
        //    //{
        //    //    await setup.InitializeAndMonitor(this).ConfigureAwait(false);
        //    //}, 10000);

        //    base.OnStart();
        //}

        private bool _isResumed;

        protected override void OnResume()
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            //Task.Run(async () => await setup.InitializeAndMonitor(this).ConfigureAwait(false));
            Task.Run(async () => await Initialize(setup).ConfigureAwait(false));

            //var handler = new Handler();

            //handler.Post(async () => 
            //{
            //    await Initialize(setup).ConfigureAwait(false);
            //    //await setup.InitializeAndMonitor(this).ConfigureAwait(false);
            //    handler.Dispose();
            //});

            base.OnResume();
            _isResumed = true;
            //var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            //await setup.InitializeAndMonitor(this).ConfigureAwait(false);
        }

        private async Task Initialize(MvxAndroidSetupSingleton setup)
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
            _isResumed = false;
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.CancelMonitor(this);
            base.OnPause();
        }

        public virtual async ValueTask InitializationComplete()
        {
            if (!_isResumed)
                return;

            await RunAppStartAsync(_bundle).ConfigureAwait(false);
        }

        protected virtual async Task RunAppStartAsync(Bundle? bundle)
        {
            if (Mvx.IoCProvider.TryResolve(out IMvxAppStart startup))
            {
                if(!startup.IsStarted)
                {
                    await startup.Start(GetAppStartHint(bundle)).ConfigureAwait(false);
                }
                else
                {
                    Finish();
                }
            }
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
