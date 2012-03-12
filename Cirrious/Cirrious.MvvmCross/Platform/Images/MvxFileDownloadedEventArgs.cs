#region Copyright
// <copyright file="MvxFileDownloadedEventArgs.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxFileDownloadedEventArgs
        : EventArgs
    {
        public MvxFileDownloadedEventArgs(string url, string downloadPath)
        {
            DownloadPath = downloadPath;
            Url = url;
        }

        public string Url { get; private set; }
        public string DownloadPath { get; private set; }
    }
}