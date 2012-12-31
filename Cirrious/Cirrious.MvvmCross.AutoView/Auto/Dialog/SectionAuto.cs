#region Copyright

// <copyright file="SectionAuto.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System.Collections;
using System.Collections.Generic;
using CrossUI.Core.Descriptions;
using CrossUI.Core.Descriptions.Dialog;

namespace Cirrious.MvvmCross.AutoView.Auto.Dialog
{
    public class SectionAuto : KeyedAuto, IEnumerable<ElementAuto>
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public List<ElementAuto> Elements { get; set; }

        public SectionAuto(string key = null, string onlyFor = null, string notFor = null, string header = null,
                           string footer = null)
            : base(key ?? "", onlyFor, notFor)
        {
            Header = header;
            Footer = footer;
            Elements = new List<ElementAuto>();
        }

        public void Add(ElementAuto elementAuto)
        {
            Elements.Add(elementAuto);
        }

        public IEnumerator<ElementAuto> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override sealed KeyedDescription ToDescription()
        {
            return ToSectionDescription();
        }

        public virtual SectionDescription ToSectionDescription()
        {
            var toReturn = new SectionDescription();
            base.Fill(toReturn);
            toReturn.Properties["Header"] = Header;
            toReturn.Properties["Footer"] = Footer;
            foreach (var elementAuto in Elements)
            {
                var elementDescription = elementAuto.ToElementDescription();
                toReturn.Elements.Add(elementDescription);
            }
            return toReturn;
        }
    }
}