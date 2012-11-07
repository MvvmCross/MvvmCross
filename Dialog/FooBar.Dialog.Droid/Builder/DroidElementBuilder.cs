using System;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class NewDroidUserInterfaceBuilder : NewKeyedUserInterfaceBuilder
    {
        public NewDroidUserInterfaceBuilder(string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName)
        {
            this.AddBuilder(typeof(IElement), new NewDroidElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IGroup), new NewDroidGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(ISection), new NewDroidSectionBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Foobar.Dialog.Core.Menus.IMenu), new NewDroidMenuBuilder(registerDefaultElements));
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder
        {
            get { return new PropertyBuilder(); }
        }
    }

    public class NewDroidElementBuilder : TypedUserInterfaceBuilder
    {
        public NewDroidElementBuilder(bool registerDefaults) 
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class NewDroidSectionBuilder : TypedUserInterfaceBuilder
    {
        public NewDroidSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class NewDroidMenuBuilder : TypedUserInterfaceBuilder
    {
        public NewDroidMenuBuilder(bool registerDefaults)
            : base(typeof(Foobar.Dialog.Core.Menus.IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }    

    public class NewDroidGroupBuilder : TypedUserInterfaceBuilder
    {
        public NewDroidGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    /*
    public class DroidElementBuilder : ElementBuilder
    {
        public DroidElementBuilder(string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName)
        {
            if (registerDefaultElements)
            {
                RegisterDefaultElements();
            }
        }

        public void RegisterDefaultElements()
        {
            RegisterConventionalKeys(typeof(DroidResources).Assembly);
        }

        protected override ISection CreateNewSection(SectionDescription sectionDescription)
        {
            return new Section();
        }

        protected override IGroup CreateNewGroup(GroupDescription groupDescription)
        {
            if (groupDescription.Key != null && groupDescription.Key != "Radio")
            {
                throw new ArgumentException("We only know about RadioGroups at present, not: " + groupDescription.Key);
            }

            return new RadioGroup();
        }
    }
     */
}