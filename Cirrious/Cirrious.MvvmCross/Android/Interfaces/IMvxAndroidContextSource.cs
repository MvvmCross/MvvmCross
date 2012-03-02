#region Copyright
// <copyright file="IMvxAndroidContextSource.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Android.Content;

namespace Cirrious.MvvmCross.Android.Platform
{
    public interface IMvxAndroidContextSource
    {
        Context Context { get; }
    }
}