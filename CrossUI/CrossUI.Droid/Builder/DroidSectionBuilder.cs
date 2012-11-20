using CrossUI.Core.Builder;
using FooBar.Dialog.Droid.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidSectionBuilder : TypedUserInterfaceBuilder
    {
        public DroidSectionBuilder(bool registerDefaults)
            : base(typeof(Section), "Section", "")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}