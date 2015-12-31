// MvxTouchBuilderRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.AutoView.iOS.Builders.Lists;
using MvvmCross.AutoView.iOS.Builders.Menus;

namespace MvvmCross.AutoView.iOS.Builders
{
    using CrossUI.Core.Elements.Lists;
    using CrossUI.Core.Elements.Menu;
    using CrossUI.iOS.Builder;

    using iOS.Builders.Lists;
    using iOS.Builders.Menus;

    public class MvxTouchBuilderRegistry : iOSBuilderRegistry
    {
        public MvxTouchBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxTouchListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxTouchListItemLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IMenu), new MvxTouchMenuBuilder(registerDefaultElements));
        }
    }
}