using System;

#warning copyright stuff - oss

namespace Cirrious.MvvmCross.Interfaces.Platform
{
#warning Is this to wp7 sepcific?
    public class BookmarkMetadata
    {
        public Uri BackgroundImageUri { get; set; }
        public string Title { get; set; }

        public Uri BackBackgroundImageUri { get; set; }
        public string BackTitle { get; set; }
        public string BackContent { get; set; }

        public int Count { get; set; }
    }
}