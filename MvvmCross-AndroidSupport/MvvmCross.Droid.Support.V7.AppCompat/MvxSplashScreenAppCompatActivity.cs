using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    [Register("mvvmcross.droid.support.v7.appcompat." + nameof(MvxSplashScreenAppCompatActivity))]
    public abstract class MvxSplashScreenAppCompatActivity
        : MvxAppCompatActivity
            , IMvxAndroidSplashScreenActivity
    {
        private const int NoContent = 0;

        private readonly int _resourceId;

        private bool _isResumed;

        protected MvxSplashScreenAppCompatActivity(int resourceId = NoContent)
        {
            _resourceId = resourceId;
        }

        public new MvxNullViewModel ViewModel
        {
            get => base.ViewModel as MvxNullViewModel;
            set => base.ViewModel = value;
        }

        public virtual void InitializationComplete()
        {
            if (!_isResumed)
                return;

            TriggerFirstNavigate();
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

        protected virtual void TriggerFirstNavigate()
        {
            var starter = Mvx.Resolve<IMvxAppStart>();
            starter.Start();
        }
    }
}