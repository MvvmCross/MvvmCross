#warning oss header

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