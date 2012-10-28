using Foobar.Dialog.Core.Builder;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidListBuilder : ListBuilder
    {
        public DroidListBuilder(string platformName = DroidConstants.PlatformName, bool registerDefaultElements = true)
            : base(platformName)
        {
            if (registerDefaultElements)
            {
                RegisterDefaultElements();
            }
        }

        public void RegisterDefaultElements()
        {
            RegisterConventionalKeys(typeof(DroidResources).Assembly);
        }
    }
}