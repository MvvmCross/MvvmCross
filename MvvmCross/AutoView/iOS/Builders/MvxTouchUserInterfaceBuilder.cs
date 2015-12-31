// MvxTouchUserInterfaceBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.iOS.Builder;

    using MvvmCross.iOS.Views;

    public class MvxTouchUserInterfaceBuilder
        : iOSUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxTouchUserInterfaceBuilder(IMvxTouchView activity, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = iOSConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            this._propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            this._propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder => this._propertyBuilder;
    }
}