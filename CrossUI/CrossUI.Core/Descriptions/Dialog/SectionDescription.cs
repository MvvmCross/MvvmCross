// SectionDescription.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Dialog
{
    public class SectionDescription : KeyedDescription
    {
        //public ElementDescription HeaderElement { get; set; }
        //public ElementDescription FooterElement { get; set; }
        public List<ElementDescription> Elements { get; set; }

        public SectionDescription()
        {
            Elements = new List<ElementDescription>();
        }
    }
}