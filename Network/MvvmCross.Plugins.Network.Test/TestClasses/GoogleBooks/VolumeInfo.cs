// VolumeInfo.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public class VolumeInfo
    {
        public string title { get; set; }
        public List<string> authors { get; set; }

        public string authorSummary
        {
            get { return authors == null ? "-" : string.Join(", ", authors); }
        }

        public string publisher { get; set; }
        public string publishedDate { get; set; }
        public string descrption { get; set; }
        public int pageCount { get; set; }
        public double averageRating { get; set; }
        public int ratingsCount { get; set; }
        public ImageLinks imageLinks { get; set; }
        public string language { get; set; }
        public string previewLink { get; set; }
        public string infoLink { get; set; }
        public string canonicalVolumeLink { get; set; }
    }
}