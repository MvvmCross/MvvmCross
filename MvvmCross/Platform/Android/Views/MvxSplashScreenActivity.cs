// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Views
{
    [Register("mvvmcross.droid.views.MvxSplashScreenActivity")]
    public abstract class MvxSplashScreenActivity
        : MvxActivity, IMvxAndroidSplashScreenActivity
    {
        private const int NoContent = 0;

        private readonly int _resourceId;

        public new MvxNullViewModel ViewModel
        {
            get { return base.ViewModel as MvxNullViewModel; }
            set { base.ViewModel = value; }
        }

        protected MvxSplashScreenActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected virtual void RequestWindowFeatures()
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
        }

        protected async override void OnCreate(Bundle bundle)
        {
            RequestWindowFeatures();

            var setup = await MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            await setup.InitializeFromSplashScreen(this);

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

        protected async override void OnResume()
        {
            base.OnResume();
            _isResumed = true;
            var setup = await MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            await setup.InitializeFromSplashScreen(this);
        }

        protected async override void OnPause()
        {
            _isResumed = false;
            var setup = await MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            await setup.RemoveSplashScreen(this);
            base.OnPause();
        }

        public async virtual Task InitializationComplete()
        {
            if (!_isResumed)
                return;

            await TriggerFirstNavigate();
        }

        protected virtual Task TriggerFirstNavigate()
        {
            var starter = Mvx.Resolve<IMvxAppStart>();
            return starter.StartAsync();
        }
    }
}
