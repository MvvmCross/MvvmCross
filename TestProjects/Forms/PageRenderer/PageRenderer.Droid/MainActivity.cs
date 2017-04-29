namespace PageRendererExample.UI.Droid
{
    [Activity(Label = "PagerenderExample.Droid", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MvxSplashScreenActivity
    {
        private bool isInitializationComplete = false;

        public override void InitializationComplete()
        {
            if (!isInitializationComplete)
            {
                isInitializationComplete = true;
                StartActivity(typeof(MvxFormsApplicationActivity));
            }
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            Forms.Init(this, bundle);

            base.OnCreate(bundle);
        }
    }
}