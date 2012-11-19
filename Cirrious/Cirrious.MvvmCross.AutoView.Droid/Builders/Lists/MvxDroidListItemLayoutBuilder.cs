using Cirrious.MvvmCross.AutoView.Droid.Interfaces.Lists;
using CrossUI.Core.Builder;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders.Lists
{
    public class MvxDroidListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxDroidListItemLayoutBuilder(bool registerDefaults)
            : base(typeof(IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}