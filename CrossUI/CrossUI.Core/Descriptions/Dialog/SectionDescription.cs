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