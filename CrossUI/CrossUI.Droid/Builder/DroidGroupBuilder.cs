using CrossUI.Core.Builder;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
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