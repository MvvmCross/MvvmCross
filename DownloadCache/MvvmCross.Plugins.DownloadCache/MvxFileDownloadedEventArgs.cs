// MvxFileDownloadedEventArgs.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.DownloadCache
{
    [Preserve(AllMembers = true)]
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