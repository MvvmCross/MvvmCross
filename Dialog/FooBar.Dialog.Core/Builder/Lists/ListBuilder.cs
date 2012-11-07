using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;
using Foobar.Dialog.Core.Lists;
using Foobar.Dialog.Core.Menus;

namespace Foobar.Dialog.Core.Builder
{
#warning Kill This dead file?
    /*
#warning Consider switching ListBuilder into two separate classes... or consider rewriting so that a keyed builder can actually just work off reflection on a general set of interfaces, classes and keys!
    public abstract class ListBuilder : BaseKeyedUserInterfaceBuilder<IBaseListLayout>
    {
        public const string StandardConventionalEnding = "ListLayout";
        public const string StandardDefaultListItemLayoutKey = "General";

        protected ListBuilder(string platformName)
            : base(platformName, StandardConventionalEnding, StandardDefaultListItemLayoutKey)
        {
        }

        public IListItemLayout Build(ListItemLayoutDescription description)
        {
            return Build((BaseListDescription) description) as IListItemLayout;
        }

        public IBaseListLayout Build(BaseListDescription description)
        {
            var instance = base.Build(description);
            if (instance == null)
            {
                return null;
            }

            FillProperties(instance, description.Properties);

            var list = instance as IBaseListLayout;

            if (description is ListLayoutDescription)
            {
                var layoutDescription = description as ListLayoutDescription;
                var listLayout = list as IListLayout;
                if (listLayout == null)
                {
                    throw new ArgumentException("ListLayoutDescription used - but IListLayout not supported in " + instance.GetType());
                }
                foreach (var childDescription in layoutDescription.ElementDescriptions)
                {
                    var layout = Build(childDescription.Value);
                    listLayout.ItemLayouts[childDescription.Key] = layout;
                }
            }

            return list;
        }
    }
     */
}