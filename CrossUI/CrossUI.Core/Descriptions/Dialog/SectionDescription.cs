#region Copyright

// <copyright file="SectionDescription.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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