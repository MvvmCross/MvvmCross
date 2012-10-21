using System.Collections.Generic;

namespace Foobar.Dialog.Core.Descriptions
{
    public class ElementDescription
    {
        public string Key { get; set; }
        public GroupDescription Group { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public List<SectionDescription> Sections { get; set; } 
    }
}