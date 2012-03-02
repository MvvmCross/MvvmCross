#region Copyright
// <copyright file="IMvxBookmarkLibrarian.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Interfaces.Platform
{
    public interface IMvxBookmarkLibrarian
    {
        bool HasBookmark(string uniqueName);
        bool AddBookmark(Type viewModelType, string uniqueName, BookmarkMetadata metadata, IDictionary<string, string> navigationArgs);
        bool UpdateBookmark(string uniqueName, BookmarkMetadata metadata);
    }
}