#region Copyright
// <copyright file="IMvxLocalFileImageLoader.cs" company="Cirrious">
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
    public interface IMvxLocalFileImageLoader<T>
    {
        MvxImage<T> Load(string localPath, bool shouldCache);
    }
}