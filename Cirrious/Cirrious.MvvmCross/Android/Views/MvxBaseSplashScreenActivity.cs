using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Android.Views
{
    public abstract class MvxBaseSplashScreenActivity
        : MvxActivityView<MvxNullViewModel>
        , IMvxServiceConsumer<IMvxStartNavigation>
    {
        private static bool _primaryInitialized = false;
        private static bool _secondaryInitialized = false;
        private static MvxBaseAndroidSetup _setup;

        private readonly int _resourceId;

        protected MvxBaseSplashScreenActivity(int resourceId)
        {
            _resourceId = resourceId;
        }

        protected abstract MvxBaseAndroidSetup CreateSetup();

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);

            if (!_primaryInitialized)
            {
                _primaryInitialized = true;

                // initialize app
                _setup = CreateSetup();
                _setup.InitializePrimary();
            }

            base.OnCreate(bundle);

            // Set our view from the "splash" layout resource
            SetContentView(_resourceId);
        }

        protected override void OnViewModelSet()
        {
            // ignored
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!_secondaryInitialized)
            {
                _secondaryInitialized = true;
                System.Threading.ThreadPool.QueueUserWorkItem((ignored) =>
                                                                  {
                                                                      _setup.InitializeSecondary();
                                                                      TriggerFirstNavigate();
                                                                  });

            }
            else
            {
                TriggerFirstNavigate();
            }
        }

        private void TriggerFirstNavigate()
        {
            // trigger the first navigate...
            var starter = this.GetService<IMvxStartNavigation>();
            starter.Start();
        }
    }
}