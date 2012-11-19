using Cirrious.MvvmCross.AutoView.Touch.Interfaces.Lists;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Lists
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