using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Builder;

namespace CustomerManagement.Droid.Views
{
    public class MvxDroidElementBuilder
        : DroidElementBuilder
    {
        public MvxDroidElementBuilder(IMvxBindingDialogActivity activity, object dataSource, string bindTag = MvxDefaultViewConstants.MvxBindTag, string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName, registerDefaultElements)
        {
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            CustomPropertySetters[bindTag] = setter;
        }
    }

    public class MvxDroidMenuBuilder
        : DroidMenuBuilder
    {
        public MvxDroidMenuBuilder(IMvxBindingDialogActivity activity, object dataSource, string bindTag = MvxDefaultViewConstants.MvxBindTag, string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName, registerDefaultElements)
        {
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            CustomPropertySetters[bindTag] = setter;
        }
    }
}