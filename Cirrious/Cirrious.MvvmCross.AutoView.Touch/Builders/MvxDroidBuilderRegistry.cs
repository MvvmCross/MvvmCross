using Cirrious.MvvmCross.Dialog.Touch.AutoView.Builders.Lists;
using FooBar.Dialog.Droid.Builder;
using Foobar.Dialog.Core.Lists;

namespace Cirrious.MvvmCross.Dialog.Touch.AutoView.Builders
{
    public class MvxTouchBuilderRegistry : TouchBuilderRegistry
    {
        public MvxTouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxTouchListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxTouchListItemLayoutBuilder(registerDefaultElements));
        }
    }
}