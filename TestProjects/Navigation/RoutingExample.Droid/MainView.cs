namespace RoutingExample.Droid
{
    [Activity(Label = "Example", MainLauncher = true, Icon = "@drawable/icon")]
    [IntentFilter(new[] {Intent.ActionView}, DataScheme = "mvx",
        Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable})]
    [IntentFilter(new[] {Intent.ActionView}, DataScheme = "https", DataHost = "mvvmcross.com",
        Categories = new[] {Intent.CategoryDefault, Intent.CategoryBrowsable})]
    public class MainView : MvxActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (!Mvx.CanResolve<IMvxNavigationService>()) return;

            var url = WebUtility.UrlDecode(intent.DataString);

            Mvx.Resolve<IMvxNavigationService>().Navigate(url);
        }
    }
}