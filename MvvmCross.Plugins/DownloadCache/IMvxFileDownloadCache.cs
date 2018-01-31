// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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