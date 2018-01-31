// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Platform;

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
