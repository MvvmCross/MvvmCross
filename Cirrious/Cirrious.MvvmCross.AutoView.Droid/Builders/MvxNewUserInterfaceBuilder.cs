using Cirrious.MvvmCross.AutoView.Droid.Builders.Lists;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Views;
using FooBar.Dialog.Droid.Builder;
using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Lists;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxNewUserInterfaceBuilder
        : NewDroidUserInterfaceBuilder
    {
        private readonly IPropertyBuilder _propertyBuilder;

#warning Need to separate out the Reflection from the instance - make more static (via injection obviously!)
        public MvxNewUserInterfaceBuilder(IMvxBindingActivity activity, object dataSource, string bindTag = MvxDefaultViewConstants.MvxBindTag, string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName, registerDefaultElements)
        {
            this.AddBuilder(typeof(IListLayout), new MvxDroidListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxDroidListItemLayoutBuilder(registerDefaultElements));
            
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