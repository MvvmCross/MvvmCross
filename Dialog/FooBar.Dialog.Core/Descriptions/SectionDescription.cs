using System.Collections.Generic;

namespace Foobar.Dialog.Core.Descriptions
{
    public class SectionDescription
    {
        public ElementDescription HeaderElement { get; set; }
        public ElementDescription FooterElement { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<ElementDescription> Elements { get; set; }
    }
}