using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Menu;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidMenuBuilder : TypedUserInterfaceBuilder
    {
        public DroidMenuBuilder(bool registerDefaults)
            : base(typeof(IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}