// BookSearchItem.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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