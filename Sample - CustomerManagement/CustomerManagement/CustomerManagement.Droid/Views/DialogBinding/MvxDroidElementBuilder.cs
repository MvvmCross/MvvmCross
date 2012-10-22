using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Builder;

namespace CustomerManagement.Droid.Views
{
    public class MvxDroidElementBuilder
        : DroidElementBuilder
    {
        public const string MvxBindTag = "MvxBind";

        public MvxDroidElementBuilder(IMvxBindingDialogActivity activity, object dataSource, string bindTag = MvxBindTag, string platformName = DroidPlatformName, bool registerDefaultElements = true)
            : base(platformName, registerDefaultElements)
        {
            var setter = new MvxBindingPropertySetter(activity, dataSource);
            CustomPropertySetters[bindTag] = setter;
        }
    }
}