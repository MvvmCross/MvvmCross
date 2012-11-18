using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Menus;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Builders.Lists
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