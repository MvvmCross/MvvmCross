// BookSearchResult.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public class BookSearchResult
    {
        public List<BookSearchItem> items { get; set; }
    }
}