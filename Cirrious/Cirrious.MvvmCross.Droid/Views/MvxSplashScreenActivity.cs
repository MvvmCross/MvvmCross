// MvxSplashScreenActivity.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Threading;
using Android.OS;
using Android.Views;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Droid.Views
{
    public abstract class MvxSplashScreenActivity
        : MvxActivity
          , IMvxAndroidSplashScreenActivity
    {
        private const int NoContent = 0;

        private static MvxAndroidSetup _setup;

        private readonly int _resourceId;
        private bool _secondStageRequested;

        public new MvxNullViewModel ViewModel
        {
            get { return (MvxNullViewModel) base.ViewModel; }
            set { base.ViewModel = value; }
        }

        protected MvxSplashScreenActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            _setup = MvxAndroidSetupSingleton.GetOrCreateSetup(ApplicationContext);

            // initialize app if necessary
            if (_setup.State == MvxSetup.MvxSetupState.Uninitialized)
            {
                _setup.InitializePrimary();
            }

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
            base.OnResume();

            if (_setup.State == MvxSetup.MvxSetupState.Initialized)
            {
                TriggerFirstNavigate();
            }
            else
            {
                if (!_secondStageRequested)
                {
                    _secondStageRequested = true;
                    ThreadPool.QueueUserWorkItem((ignored) =>
                        {
                            _setup.InitializeSecondary();
                            RunOnUiThread(OnInitialisationComplete);
                        });
                }
            }
        }

        protected virtual void OnInitialisationComplete()
        {
            TriggerFirstNavigate();
        }

        protected virtual void TriggerFirstNavigate()
        {
            var starter = Mvx.Resolve<IMvxAppStart>();
            starter.Start();
        }
    }
}