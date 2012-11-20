using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Dialog
{
    public class ElementDescription : KeyedDescription
    {
        public GroupDescription Group { get; set; }
        public List<SectionDescription> Sections { get; set; }

        public ElementDescription()
        {
            Sections = new List<SectionDescription>();
        }
    }
}