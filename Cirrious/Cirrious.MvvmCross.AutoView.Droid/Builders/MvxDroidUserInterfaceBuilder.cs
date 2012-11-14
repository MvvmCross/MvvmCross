using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using FooBar.Dialog.Droid.Builder;
using Foobar.Dialog.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidUserInterfaceBuilder
        : DroidUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

        public MvxDroidUserInterfaceBuilder(IMvxBindingActivity activity, object dataSource, IBuilderRegistry builderRegistry, string bindTag = MvxAutoViewConstants.MvxBindTag, string platformName = DroidConstants.PlatformName)
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