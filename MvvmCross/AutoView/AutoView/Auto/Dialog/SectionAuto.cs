// SectionAuto.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Auto.Dialog
{
    using System.Collections;
    using System.Collections.Generic;

    using CrossUI.Core.Descriptions;
    using CrossUI.Core.Descriptions.Dialog;

    public class SectionAuto : KeyedAuto, IEnumerable<ElementAuto>
    {
        public string Header { get; set; }
        public string Footer { get; set; }
        public List<ElementAuto> Elements { get; set; }

        public SectionAuto(string key = null, string onlyFor = null, string notFor = null, string header = null,
                           string footer = null)
            : base(key ?? "", onlyFor, notFor)
        {
            this.Header = header;
            this.Footer = footer;
            this.Elements = new List<ElementAuto>();
        }

        public void Add(ElementAuto elementAuto)
        {
            this.Elements.Add(elementAuto);
        }

        public IEnumerator<ElementAuto> GetEnumerator()
        {
            return this.Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public sealed override KeyedDescription ToDescription()
        {
            return this.ToSectionDescription();
        }

        public virtual SectionDescription ToSectionDescription()
        {
            var toReturn = new SectionDescription();
            base.Fill(toReturn);
            toReturn.Properties["Header"] = this.Header;
            toReturn.Properties["Footer"] = this.Footer;
            foreach (var elementAuto in this.Elements)
            {
                var elementDescription = elementAuto.ToElementDescription();
                toReturn.Elements.Add(elementDescription);
            }
            return toReturn;
        }
    }
}