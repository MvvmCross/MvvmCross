// HtmlActivity.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.App;
using Android.OS;
using Android.Webkit;

namespace CrossUI.Droid.Dialog.Elements
{
    [Activity(Name = "crossui.droid.dialog.elements.HtmlActivity")]
    public class HtmlActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string url = Intent.GetStringExtra("URL");
            Title = Intent.GetStringExtra("Title");

            var webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.BuiltInZoomControls = true;
            SetContentView(webview);
            webview.LoadUrl(url);
        }
    }
}