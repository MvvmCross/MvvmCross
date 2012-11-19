using Cirrious.MvvmCross.AutoView.Touch.Builders.Lists;
using Cirrious.MvvmCross.AutoView.Touch.Builders.Menus;
using CrossUI.Core.Elements.Lists;
using CrossUI.Core.Elements.Menu;
using FooBar.Dialog.Droid.Builder;

namespace Cirrious.MvvmCross.AutoView.Touch.Builders
{
    public class MvxTouchBuilderRegistry : TouchBuilderRegistry
    {
        public MvxTouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxTouchListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxTouchListItemLayoutBuilder(registerDefaultElements));
			this.AddBuilder(typeof(IMenu), new MvxTouchMenuBuilder(registerDefaultElements));
		}
    }
}