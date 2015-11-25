// MvxBookmarkMetadata.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Bookmarks
{
#warning this bookmark code may be too wp7 sepcific?

    public class MvxBookmarkMetadata
    {
        public Uri BackgroundImageUri { get; set; }
        public string Title { get; set; }

        public Uri BackBackgroundImageUri { get; set; }
        public string BackTitle { get; set; }
        public string BackContent { get; set; }

        public int Count { get; set; }
    }
}