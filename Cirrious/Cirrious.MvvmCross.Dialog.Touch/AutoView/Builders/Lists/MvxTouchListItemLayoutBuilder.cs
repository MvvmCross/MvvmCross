using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using Foobar.Dialog.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders.Lists
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