using Cirrious.MvvmCross.AutoView.Droid.Builders.Lists;
using FooBar.Dialog.Droid.Builder;
using Foobar.Dialog.Core.Lists;

namespace Cirrious.MvvmCross.AutoView.Droid.Builders
{
    public class MvxDroidBuilderRegistry : DroidBuilderRegistry
    {
        public MvxDroidBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxDroidListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxDroidListItemLayoutBuilder(registerDefaultElements));
        }
    }
}