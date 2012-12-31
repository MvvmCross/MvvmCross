#region Copyright

// <copyright file="HtmlActivity.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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

            var webview = new WebView(this);
            webview.Settings.JavaScriptEnabled = true;
            webview.Settings.BuiltInZoomControls = true;
            SetContentView(webview);
            webview.LoadUrl(url);
        }
    }
}