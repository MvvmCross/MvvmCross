// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugins.Network.Test.TestClasses.GoogleBooks
{
    public class BookSearchItem
    {
        public string kind { get; set; }
        public string id { get; set; }
        public VolumeInfo volumeInfo { get; set; }

        public override string ToString()
        {
            return volumeInfo.title;
        }
    }
}