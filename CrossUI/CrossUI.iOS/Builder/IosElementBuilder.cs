// iOSElementBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.iOS.Dialog.Elements;

namespace CrossUI.iOS.Builder
{
    public class IosBuilderRegistry : BuilderRegistry
    {
        public IosBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(Element), new IosElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Group), new IosGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Section), new IosSectionBuilder(registerDefaultElements));
        }
    }

    public class IosUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public IosUserInterfaceBuilder(IBuilderRegistry registry, string platformName = IosConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder => new PropertyBuilder();
    }

    public class IosElementBuilder : TypedUserInterfaceBuilder
    {
        public IosElementBuilder(bool registerDefaults)
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class IosSectionBuilder : TypedUserInterfaceBuilder
    {
        public IosSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }

    public class IosGroupBuilder : TypedUserInterfaceBuilder
    {
        public IosGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}