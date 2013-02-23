// MvxWebBrowserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using Cirrious.MvvmCross.Touch.Platform.Tasks;
using MonoTouch.Foundation;

#endregion

namespace Cirrious.MvvmCross.Plugins.WebBrowser.Touch
{
    public class MvxWebBrowserTask : MvxTouchTask, IMvxWebBrowserTask
    {
        #region IMvxWebBrowserTask Members

        public void ShowWebPage(string url)
        {
            var nsurl = new NSUrl(url);
            DoUrlOpen(nsurl);
        }

        #endregion
    }
}