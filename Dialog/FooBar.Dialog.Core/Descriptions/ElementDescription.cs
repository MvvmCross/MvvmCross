using System.Collections.Generic;

namespace Foobar.Dialog.Core.Descriptions
{
    public class ElementDescription : BaseDescription
    {
        public string Key { get; set; }
        public GroupDescription Group { get; set; }
        public List<SectionDescription> Sections { get; set; } 
    }
}