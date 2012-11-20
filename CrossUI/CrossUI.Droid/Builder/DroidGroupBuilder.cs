using CrossUI.Core.Builder;
using FooBar.Dialog.Droid.Elements;

namespace FooBar.Dialog.Droid.Builder
{
    public class DroidGroupBuilder : TypedUserInterfaceBuilder
    {
        public DroidGroupBuilder(bool registerDefaults)
            : base(typeof(Group), "Group", "Radio")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}