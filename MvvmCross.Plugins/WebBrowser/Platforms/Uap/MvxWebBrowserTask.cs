// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#region using

using System;
using Windows.System;

#endregion using

namespace MvvmCross.Plugin.WebBrowser.Platforms.Uap
{
    public class MvxWebBrowserTask : IMvxWebBrowserTask
    {
        #region IMvxWebBrowserTask Members

        public void ShowWebPage(string url)
        {
            // note - this call deliberately not awaited - OK to continue with flow
            Launcher.LaunchUriAsync(new Uri(url));
        }

        #endregion IMvxWebBrowserTask Members
    }
}
