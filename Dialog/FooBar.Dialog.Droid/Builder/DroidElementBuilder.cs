using System;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Descriptions;
using Foobar.Dialog.Core.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class NewDroidUserInterfaceBuilder : NewKeyedUserInterfaceBuilder
    {
        public NewDroidUserInterfaceBuilder(string platformName = DroidConstants.PlatformName)
            : base(platformName)
        {

        }
    }

    public class NewDroidElementBuilder : TypedUserInterfaceBuilder
    {
        
    }

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
}