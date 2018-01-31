// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugins.DownloadCache
{
    public interface IMvxHttpFileDownloader
    {
        void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error);
    }
}