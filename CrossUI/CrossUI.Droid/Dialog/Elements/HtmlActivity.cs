using Android.App;
using Android.OS;
using Android.Webkit;

namespace CrossUI.Droid.Dialog.Elements
{
    [Activity]
    public class HtmlActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string url = Intent.GetStringExtra("URL");
            Title = Intent.GetStringExtra("Title");

            WebView webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.BuiltInZoomControls = true;
            SetContentView(webview);
            webview.LoadUrl(url);
        }
    }
}