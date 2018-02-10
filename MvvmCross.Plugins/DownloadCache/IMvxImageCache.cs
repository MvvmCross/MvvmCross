// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MvvmCross.Plugin.DownloadCache
{
    public interface IMvxImageCache<T>
    {
        bool ContainsImage(string url);
        Task<T> RequestImage(string url);
    }
}
