// BooksService.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

namespace MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public static class BooksService
    {
        public static string GetSearchUrl(string whatFor)
        {
            string address = string.Format("https://www.googleapis.com/books/v1/volumes?q={0}",
                                           Uri.EscapeDataString(whatFor));
            return address;
        }
    }
}