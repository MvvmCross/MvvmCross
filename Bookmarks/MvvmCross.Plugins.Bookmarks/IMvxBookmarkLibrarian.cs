// IMvxBookmarkLibrarian.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace MvvmCross.Plugins.Bookmarks
{
    public interface IMvxBookmarkLibrarian
    {
        bool HasBookmark(string uniqueName);

        bool AddBookmark(Type viewModelType, string uniqueName, MvxBookmarkMetadata metadata,
                         IDictionary<string, string> navigationArgs);

        bool UpdateBookmark(string uniqueName, MvxBookmarkMetadata metadata);
    }
}