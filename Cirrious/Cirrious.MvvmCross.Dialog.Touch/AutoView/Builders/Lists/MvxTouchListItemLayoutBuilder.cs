using Cirrious.MvvmCross.Dialog.Touch.AutoView.Interfaces.Lists;
using Foobar.Dialog.Core.Builder;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Builders.Lists
{
    public class MvxTouchListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListItemLayoutBuilder(bool registerDefaults)
            : base(typeof(IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}