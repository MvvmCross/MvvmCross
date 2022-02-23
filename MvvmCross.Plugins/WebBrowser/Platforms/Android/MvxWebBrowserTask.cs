// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using Android.Net;
using MvvmCross.Platforms.Android;

namespace MvvmCross.Plugin.WebBrowser.Platforms.Android
{
    [Preserve(AllMembers = true)]
    public class MvxWebBrowserTask : MvxAndroidTask, IMvxWebBrowserTask
    {
        public void ShowWebPage(string url)
        {
            var intent = new Intent(Intent.ActionView, Uri.Parse(url));
            StartActivity(intent);
        }
    }
}
