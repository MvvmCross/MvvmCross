// iOSElementBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.iOS.Dialog.Elements;

namespace CrossUI.iOS.Builder
{
    public class iOSBuilderRegistry : BuilderRegistry
    {
        public iOSBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(Element), new iOSElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Group), new iOSGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Section), new iOSSectionBuilder(registerDefaultElements));
        }
    }

    public class iOSUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public iOSUserInterfaceBuilder(IBuilderRegistry registry, string platformName = iOSConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder => new PropertyBuilder();
    }

    public class iOSElementBuilder : TypedUserInterfaceBuilder
    {
        public iOSElementBuilder(bool registerDefaults)
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class iOSSectionBuilder : TypedUserInterfaceBuilder
    {
        public iOSSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class iOSGroupBuilder : TypedUserInterfaceBuilder
    {
        public iOSGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}