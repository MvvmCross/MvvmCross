// IMvxLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public interface IMvxLocalFileImageLoader<T>
    {
        void Load(string localPath, bool shouldCache, int width, int height, Action<MvxImage<T>> success, Action<Exception> error);
    }
}