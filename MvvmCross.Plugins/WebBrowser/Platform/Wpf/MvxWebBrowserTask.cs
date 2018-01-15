﻿// MvxWebBrowserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#region using

using System.Diagnostics;

#endregion using

namespace MvvmCross.Plugins.WebBrowser.Wpf
{
    public class MvxWebBrowserTask : IMvxWebBrowserTask
    {
        #region IMvxWebBrowserTask Members

        public void ShowWebPage(string url)
        {
            // note - this call deliberately not awaited - OK to continue with flow
            Process.Start(url);
        }

        #endregion IMvxWebBrowserTask Members
    }
}