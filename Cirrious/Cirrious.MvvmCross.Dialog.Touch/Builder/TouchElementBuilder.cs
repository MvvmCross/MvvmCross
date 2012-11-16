using System;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class TouchBuilderRegistry : BuilderRegistry
    {
        public TouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(Element), new TouchElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Group), new TouchGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Section), new TouchSectionBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Foobar.Dialog.Core.Menus.IMenu), new TouchMenuBuilder(registerDefaultElements));
        }
    }

    public class TouchUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public TouchUserInterfaceBuilder(IBuilderRegistry registry, string platformName = TouchConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder
        {
            get { return new PropertyBuilder(); }
        }
    }

    public class TouchElementBuilder : TypedUserInterfaceBuilder
    {
        public TouchElementBuilder(bool registerDefaults) 
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class TouchSectionBuilder : TypedUserInterfaceBuilder
    {
        public TouchSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class TouchMenuBuilder : TypedUserInterfaceBuilder
    {
        public TouchMenuBuilder(bool registerDefaults)
            : base(typeof(Foobar.Dialog.Core.Menus.IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }    

    public class TouchGroupBuilder : TypedUserInterfaceBuilder
    {
        public TouchGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}