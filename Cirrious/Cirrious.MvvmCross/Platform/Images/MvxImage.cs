#region Copyright
// <copyright file="MvxImage.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion
namespace Cirrious.MvvmCross.Platform.Images
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