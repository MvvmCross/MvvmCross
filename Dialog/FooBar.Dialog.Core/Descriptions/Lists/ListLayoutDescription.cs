using System;
using System.Collections;
using System.Collections.Generic;
using Foobar.Dialog.Core.Lists;

namespace Foobar.Dialog.Core.Descriptions
{
    public class BuilderAttribute : Attribute
    {
        public Type TargetInterfaceType { get; private set; }

        public BuilderAttribute(Type targetInterfaceType)
        {
            TargetInterfaceType = targetInterfaceType;
        }
    }

    public class ListLayoutDescription : BaseListDescription
    {
        [Builder(typeof(IListItemLayout))]
        public Dictionary<string, ListItemLayoutDescription> ElementDescriptions { get; set; }
    }
}