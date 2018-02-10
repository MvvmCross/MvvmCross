// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;
using MvvmCross.Plugin.File;

namespace MvvmCross.Plugin.DownloadCache
{
    [Preserve(AllMembers = true)]
	public static class MvxFileStoreHelper
    {
        public static IMvxFileStore SafeGetFileStore()
        {
            IMvxFileStore toReturn;
            if (Mvx.TryResolve(out toReturn))
                return toReturn;

            throw new MvxException("You must call EnsureLoaded on the File plugin before using the DownloadCache");
        }
    }
}
