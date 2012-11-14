using System.Collections.Generic;

namespace Foobar.Dialog.Core.Descriptions
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