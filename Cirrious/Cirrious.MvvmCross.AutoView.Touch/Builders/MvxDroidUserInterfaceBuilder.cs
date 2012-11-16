using Cirrious.MvvmCross.AutoView;
using Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces;
using FooBar.Dialog.Droid.Builder;
using Foobar.Dialog.Core.Builder;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Builders
{
    public class MvxTouchUserInterfaceBuilder
        : TouchUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxTouchUserInterfaceBuilder(IMvxBindingViewController activity, object dataSource, IBuilderRegistry builderRegistry, string bindTag = MvxAutoViewConstants.MvxBindTag, string platformName = TouchConstants.PlatformName)
            : base(builderRegistry, platformName)
        {
            _propertyBuilder = new PropertyBuilder();
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            _propertyBuilder.CustomPropertySetters[bindTag] = setter;
        }

        protected override Foobar.Dialog.Core.Builder.IPropertyBuilder PropertyBuilder
        {
            get
            {
                return _propertyBuilder;
            }
        }
    }
}