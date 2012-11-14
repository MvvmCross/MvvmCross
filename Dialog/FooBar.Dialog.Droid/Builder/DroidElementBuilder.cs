using System;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidBuilderRegistry : BuilderRegistry
    {
        public DroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IElement), new DroidElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IGroup), new DroidGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(ISection), new DroidSectionBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Foobar.Dialog.Core.Menus.IMenu), new DroidMenuBuilder(registerDefaultElements));
        }
    }

    public class DroidUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public DroidUserInterfaceBuilder(IBuilderRegistry registry, string platformName = DroidConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder
        {
            get { return new PropertyBuilder(); }
        }
    }

    public class DroidElementBuilder : TypedUserInterfaceBuilder
    {
        public DroidElementBuilder(bool registerDefaults) 
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class DroidSectionBuilder : TypedUserInterfaceBuilder
    {
        public DroidSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class DroidMenuBuilder : TypedUserInterfaceBuilder
    {
        public DroidMenuBuilder(bool registerDefaults)
            : base(typeof(Foobar.Dialog.Core.Menus.IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }    

    public class DroidGroupBuilder : TypedUserInterfaceBuilder
    {
        public DroidGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}