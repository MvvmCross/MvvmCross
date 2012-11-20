using CrossUI.Core.Builder;
using CrossUI.Core.Elements.Lists;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders.Lists
{
    public class MvxTouchListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListLayoutBuilder(bool registerDefaults)
            : base(typeof(IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}