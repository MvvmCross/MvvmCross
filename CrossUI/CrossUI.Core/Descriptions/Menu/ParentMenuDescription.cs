using System.Collections.Generic;

namespace CrossUI.Core.Descriptions.Menu
{
    public class ParentMenuDescription : MenuDescription
    {
        public List<MenuDescription> Children { get; set; }

        public ParentMenuDescription()
        {
            Children = new List<MenuDescription>();
        }
    }
}