// MvxImage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Plugins.DownloadCache
{
    public abstract class MvxImage<T>
    {
        protected MvxImage(T rawImage)
        {
            RawImage = rawImage;
        }

        public T RawImage { get; private set; }

        public abstract int GetSizeInBytes();
    }
}