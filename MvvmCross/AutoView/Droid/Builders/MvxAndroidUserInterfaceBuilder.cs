// MvxAndroidUserInterfaceBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Builders
{
    using CrossUI.Core.Builder;
    using CrossUI.Droid.Builder;

    public class MvxAndroidUserInterfaceBuilder
        : DroidUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxAndroidUserInterfaceBuilder(IMvxAndroidBindingContext context, object dataSource,
                                            IBuilderRegistry builderRegistry,
                                            string bindTag = MvxAutoViewConstants.MvxBindTag,
                                            string platformName = DroidConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            this._propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(context, dataSource);
            this._propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override IPropertyBuilder PropertyBuilder => this._propertyBuilder;
    }
}