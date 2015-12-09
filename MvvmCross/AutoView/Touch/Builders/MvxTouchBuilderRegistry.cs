// MvxTouchBuilderRegistry.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Builders
{
    using CrossUI.Core.Elements.Lists;
    using CrossUI.Core.Elements.Menu;
    using CrossUI.Touch.Builder;

    using MvvmCross.AutoView.Touch.Builders.Lists;
    using MvvmCross.AutoView.Touch.Builders.Menus;

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