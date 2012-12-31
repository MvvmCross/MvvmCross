#region Copyright

// <copyright file="MvxDroidUserInterfaceBuilder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using CrossUI.Core.Builder;
using CrossUI.Droid.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidUserInterfaceBuilder
        : DroidUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxDroidUserInterfaceBuilder(IMvxBindingActivity activity, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = DroidConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            _propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            _propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder
        {
            get { return _propertyBuilder; }
        }
    }
}