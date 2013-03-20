// MvxWebBrowserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Cirrious.CrossCore.Droid.Platform;

namespace Cirrious.MvvmCross.Plugins.WebBrowser.Droid
{
    public class MvxWebBrowserTask : MvxAndroidTask, IMvxWebBrowserTask
    {
        public void ShowWebPage(string url)
        {
            var intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse(url));
            StartActivity(intent);
        }
    }
}