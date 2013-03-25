﻿#region Copyright
// <copyright file="MvxWebBrowserTask.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

#if false

#warning removed as its not really useful currently

#region using

using Cirrious.MvvmCross.Interfaces.Platform.Tasks;

#endregion

namespace Cirrious.MvvmCross.Pss.Services.Tasks
{
    public class MvxWebBrowserTask : IMvxWebBrowserTask
    {
        #region IMvxWebBrowserTask Members

        public void ShowWebPage(string url)
        {
            System.Pss.WriteLine("Opening a browser on {0}", url);
            System.Diagnostics.Process.Start(url);
        }

        #endregion
    }
}

#endif