// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;

namespace MvvmCross.Plugins.Network.UnitTest.TestClasses.GoogleBooks
{
    public static class BooksService
    {
        public static string GetSearchUrl(string whatFor)
        {
            string address = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(whatFor)}";
            return address;
        }
    }
}
