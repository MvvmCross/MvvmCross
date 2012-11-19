using CrossUI.Core.Builder;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidUserInterfaceBuilder : KeyedUserInterfaceBuilder
    {
        public DroidUserInterfaceBuilder(IBuilderRegistry registry, string platformName = DroidConstants.PlatformName)
            : base(platformName, registry)
        {
        }

        // default implementation...
        protected override IPropertyBuilder PropertyBuilder
        {
            get { return new PropertyBuilder(); }
        }
    }
}