// DroidUserInterfaceBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CrossUI.Core.Builder;

namespace CrossUI.Droid.Builder
{
    public class DroidUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public DroidUserInterfaceBuilder(IBuilderRegistry registry, string platformName = DroidConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder => new PropertyBuilder();
    }
}