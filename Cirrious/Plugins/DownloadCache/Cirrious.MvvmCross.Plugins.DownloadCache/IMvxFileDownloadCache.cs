#region Copyright

// <copyright file="IMvxFileDownloadCache.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public interface IMvxFileDownloadCache
    {
        void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error);
    }
}