#region Copyright
// <copyright file="MvxBookmarkMetadata.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;

namespace Cirrious.MvvmCross.Plugins.Bookmarks
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