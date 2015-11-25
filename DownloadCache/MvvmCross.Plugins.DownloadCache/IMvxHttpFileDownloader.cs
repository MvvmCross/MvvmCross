// IMvxHttpFileDownloader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.DownloadCache
{
    public interface IMvxHttpFileDownloader
    {
        void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error);
    }
}