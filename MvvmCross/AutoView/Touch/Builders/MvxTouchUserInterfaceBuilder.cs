// MvxTouchUserInterfaceBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.Touch.Builder;

    using MvvmCross.Touch.Views;

    public class MvxTouchUserInterfaceBuilder
        : TouchUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxTouchUserInterfaceBuilder(IMvxTouchView activity, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = TouchConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            this._propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            this._propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder => this._propertyBuilder;
    }
}