using CrossUI.Core.Builder;
using CrossUI.Droid.Dialog.Elements;

namespace CrossUI.Droid.Builder
{
    public class DroidElementBuilder : TypedUserInterfaceBuilder
    {
        public DroidElementBuilder(bool registerDefaults) 
            : base(typeof(Element), "Element", "String")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}