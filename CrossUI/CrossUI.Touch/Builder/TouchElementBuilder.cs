// TouchElementBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;
using CrossUI.Touch.Dialog.Elements;

namespace CrossUI.Touch.Builder
{
    public class TouchBuilderRegistry : BuilderRegistry
    {
        public TouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(Element), new TouchElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Group), new TouchGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof(Section), new TouchSectionBuilder(registerDefaultElements));
        }
    }

    public class TouchUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public TouchUserInterfaceBuilder(IBuilderRegistry registry, string platformName = TouchConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder => new PropertyBuilder();
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