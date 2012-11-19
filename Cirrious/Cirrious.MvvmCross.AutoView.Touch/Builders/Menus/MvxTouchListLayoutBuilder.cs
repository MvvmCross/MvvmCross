using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Menu;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Menus
{
    public class MvxTouchMenuBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchMenuBuilder(bool registerDefaults)
            : base(typeof(IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}