#region Copyright

// <copyright file="TouchElementBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using CrossUI.Core.Builder;
using CrossUI.Touch.Dialog.Elements;

namespace CrossUI.Touch.Builder
{
    public class TouchBuilderRegistry : BuilderRegistry
    {
        public TouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof (Element), new TouchElementBuilder(registerDefaultElements));
            this.AddBuilder(typeof (Group), new TouchGroupBuilder(registerDefaultElements));
            this.AddBuilder(typeof (Section), new TouchSectionBuilder(registerDefaultElements));
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
            : base(typeof (Element), "Element", "String")
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
            : base(typeof (Section), "Section", "")
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
            : base(typeof (Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}