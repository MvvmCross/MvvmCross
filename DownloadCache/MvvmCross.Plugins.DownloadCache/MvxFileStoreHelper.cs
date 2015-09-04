// MvxFileStoreHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using MvvmCross.Plugins.File;

namespace MvvmCross.Plugins.DownloadCache
{
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