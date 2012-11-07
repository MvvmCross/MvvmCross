using Foobar.Dialog.Core.Builder;
using Foobar.Dialog.Core.Lists;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders.Lists
{
    public class MvxDroidListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxDroidListLayoutBuilder(bool registerDefaults)
            : base(typeof(IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}