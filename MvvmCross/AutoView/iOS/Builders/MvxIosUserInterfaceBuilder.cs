// MvxIosUserInterfaceBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.iOS.Builder;

    using MvvmCross.iOS.Views;

    public class MvxIosUserInterfaceBuilder
        : IosUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxIosUserInterfaceBuilder(IMvxIosView activity, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = IosConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            this._propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            this._propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder => this._propertyBuilder;
    }
}