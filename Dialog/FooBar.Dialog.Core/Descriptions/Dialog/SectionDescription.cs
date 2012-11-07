using System.Collections.Generic;

namespace Foobar.Dialog.Core.Descriptions
{
    public class SectionDescription : KeyedDescription
    {
        public ElementDescription HeaderElement { get; set; }
        public ElementDescription FooterElement { get; set; }
        public List<ElementDescription> Elements { get; set; }
    }
}