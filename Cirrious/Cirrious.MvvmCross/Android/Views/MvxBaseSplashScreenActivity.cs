#region Copyright
// <copyright file="MvxBaseSplashScreenActivity.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Threading;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Android.Views
{
    public abstract class MvxBaseSplashScreenActivity
        : MvxActivityView<MvxNullViewModel>
        , IMvxAndroidSplashScreenActivity
        , IMvxServiceConsumer<IMvxStartNavigation>
    {
        private const int NoContent = 0;

        private static bool _primaryInitialized = false;
        private static MvxBaseAndroidSetup _setup;

        private readonly int _resourceId;

        protected MvxBaseSplashScreenActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            _setup = MvxAndroidSetupSingleton.GetOrCreateSetup(ApplicationContext);

            if (!_primaryInitialized)
            {
                _primaryInitialized = true;

                // initialize app if necessary
                if (_setup.State == MvxBaseSetup.MvxSetupState.Uninitialized)
                {
                    _setup.InitializePrimary();
                }
            }

            base.OnCreate(bundle);

            if (_resourceId != NoContent)
            {
                // Set our view from the "splash" layout resource
                SetContentView(_resourceId);
            }
        }

        protected override void OnViewModelSet()
        {
            // ignored
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (_setup.State == MvxBaseSetup.MvxSetupState.Initialized)
            {
                TriggerFirstNavigate();
            }
            else
            {
                ThreadPool.QueueUserWorkItem((ignored) =>
                {
                    _setup.InitializeSecondary();
                    TriggerFirstNavigate();
                });
            }
        }

        private void TriggerFirstNavigate()
        {
            var starter = this.GetService<IMvxStartNavigation>();
            starter.Start();
        }
    }
}