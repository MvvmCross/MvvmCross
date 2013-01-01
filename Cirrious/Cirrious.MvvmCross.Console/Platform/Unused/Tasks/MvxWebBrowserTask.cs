// MvxWebBrowserTask.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if false

#warning removed as its not really useful currently

#region using

using Cirrious.MvvmCross.Interfaces.Platform.Tasks;

#endregion

namespace Cirrious.MvvmCross.Console.Services.Tasks
{
    public class MvxWebBrowserTask : IMvxWebBrowserTask
    {
        #region IMvxWebBrowserTask Members

        public void ShowWebPage(string url)
        {
            System.Console.WriteLine("Opening a browser on {0}", url);
            System.Diagnostics.Process.Start(url);
        }

        #endregion
    }
}

#endif