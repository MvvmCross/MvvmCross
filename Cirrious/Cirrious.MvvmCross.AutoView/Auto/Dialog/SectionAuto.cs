// SectionAuto.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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

        public sealed override KeyedDescription ToDescription()
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