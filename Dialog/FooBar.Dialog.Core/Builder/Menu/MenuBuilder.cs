using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Menus;

namespace Foobar.Dialog.Core.Builder
{
    public abstract class MenuBuilder : BaseKeyedUserInterfaceBuilder<IMenu>
    {
        public const string StandardConventionalEnding = "Menu";
        public const string StandardDefaultMenuKey = "CaptionAndIcon";

        protected MenuBuilder(string platformName)
            : base(platformName, StandardConventionalEnding, StandardDefaultMenuKey)
        {
        }

        public IMenu Build(MenuDescription description)
        {
            var instance = base.Build(description);
            if (instance == null)
            {
                return null;
            }

            FillProperties(instance, description.Properties);

            var menu = instance as IMenu;

            if (description is ParentMenuDescription)
            {
                var parentDescription = description as ParentMenuDescription;
                var parentMenu = menu as IParentMenu;
                if (parentMenu == null)
                {
                    throw new ArgumentException("ParentMenuDescription used - but IParentMenu not supported in " + instance.GetType());
                }
                foreach (var childDescription in parentDescription.Children)
                {
                    var child = Build(childDescription);
                    if (child != null)
                    {
                        parentMenu.Children.Add(child);
                    }
                }
            }

            return menu;
        }
    }
}