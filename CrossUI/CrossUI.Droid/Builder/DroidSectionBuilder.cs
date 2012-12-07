using CrossUI.Core.Builder;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
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