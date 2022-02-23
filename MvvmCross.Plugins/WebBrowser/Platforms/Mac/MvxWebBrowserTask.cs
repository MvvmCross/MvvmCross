// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Foundation;
using MvvmCross.Platforms.Mac;

namespace MvvmCross.Plugin.WebBrowser.Platforms.Mac
{
    [Preserve(AllMembers = true)]
    public class MvxWebBrowserTask : MvxMacTask, IMvxWebBrowserTask
    {
        public void ShowWebPage(string url)
        {
            var nsurl = new NSUrl(url);
            DoUrlOpen(nsurl);
        }
    }
}
