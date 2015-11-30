// MvxSplashScreenActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.OS;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxSplashScreenCompatActivity
        : MvxAppCompatActivity
          , IMvxAndroidSplashScreenActivity
    {
        private const int NoContent = 0;

        private readonly int _resourceId;

        public new MvxNullViewModel ViewModel
        {
            get { return base.ViewModel as MvxNullViewModel; }
            set { base.ViewModel = value; }
        }

        protected MvxSplashScreenCompatActivity(int resourceId = NoContent)
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
            setup.InitializeFromSplashScreen(this);

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
            setup.InitializeFromSplashScreen(this);
        }

        protected override void OnPause()
        {
            _isResumed = false;
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.RemoveSplashScreen(this);
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
            var starter = Mvx.Resolve<IMvxAppStart>();
            starter.Start();
        }
    }
}