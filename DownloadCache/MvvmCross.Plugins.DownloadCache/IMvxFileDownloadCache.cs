// IMvxFileDownloadCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.DownloadCache
{
    public interface IMvxFileDownloadCache
    {
        void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error);

        void ClearAll();

        void Clear(string httpSource);
    }
}