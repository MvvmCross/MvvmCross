#region Copyright

// <copyright file="DroidUserInterfaceBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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
        protected override IPropertyBuilder PropertyBuilder
        {
            get { return new PropertyBuilder(); }
        }
    }
}