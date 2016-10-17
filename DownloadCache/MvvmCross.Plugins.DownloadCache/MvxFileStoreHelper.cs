// MvxFileStoreHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Plugins.File;

namespace MvvmCross.Plugins.DownloadCache
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