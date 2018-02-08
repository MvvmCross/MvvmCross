// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Base;

namespace MvvmCross.Plugin.DownloadCache
{
    [Preserve(AllMembers = true)]
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
