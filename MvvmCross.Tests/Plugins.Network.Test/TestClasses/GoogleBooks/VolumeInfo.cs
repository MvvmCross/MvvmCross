// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public class VolumeInfo
    {
        public string title { get; set; }
        public List<string> authors { get; set; }

        public string authorSummary => authors == null ? "-" : string.Join(", ", authors);

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