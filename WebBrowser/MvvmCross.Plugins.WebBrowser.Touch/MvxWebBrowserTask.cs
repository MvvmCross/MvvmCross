// MvxWebBrowserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Platform;
using Foundation;

namespace MvvmCross.Plugins.WebBrowser.Touch
{
    public class MvxWebBrowserTask : MvxTouchTask, IMvxWebBrowserTask
    {
        public void ShowWebPage(string url)
        {
            var nsurl = new NSUrl(url);
            DoUrlOpen(nsurl);
        }
    }
}