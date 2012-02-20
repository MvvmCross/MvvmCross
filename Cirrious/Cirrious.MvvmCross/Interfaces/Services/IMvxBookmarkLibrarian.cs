#warning oss header

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.ViewModels;

namespace Cirrious.MvvmCross.Interfaces.Services
{
    public interface IMvxBookmarkLibrarian
    {
        bool HasBookmark(string uniqueName);
        bool AddBookmark(Type viewModelType, string uniqueName, BookmarkMetadata metadata, IDictionary<string, string> navigationArgs);
        bool UpdateBookmark(string uniqueName, BookmarkMetadata metadata);
    }
}