using FooBar.Dialog.Droid;
using FooBar.Dialog.Droid.Builder;

namespace CustomerManagement.Droid.Views
{
    public class MvxDroidElementBuilder
        : DroidElementBuilder
    {
        public const string MvxBindTag = "MvxBind";

        private static void EnsureResourcesInitialised()
        {
            if (!_resourcesInitialised)
            {
                DroidResources.Initialise(typeof(Resource.Layout));
                _resourcesInitialised = true;
            }
        }

        private static bool _resourcesInitialised = false;

        public MvxDroidElementBuilder(IMvxBindingDialogActivity activity, object dataSource, string bindTag = MvxBindTag, bool registerDefaultElements = true)
            : base(registerDefaultElements)
        {
            EnsureResourcesInitialised();

            var setter = new MvxBindingPropertySetter(activity, dataSource);
            CustomPropertySetters[bindTag] = setter;
        }
    }
}