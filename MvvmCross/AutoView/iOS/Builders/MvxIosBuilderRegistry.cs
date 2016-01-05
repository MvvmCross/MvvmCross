// MvxIosBuilderRegistry.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders
{
    using CrossUI.Core.Elements.Lists;
    using CrossUI.Core.Elements.Menu;
    using CrossUI.iOS.Builder;

    using iOS.Builders.Lists;
    using iOS.Builders.Menus;

    public class MvxIosBuilderRegistry : IosBuilderRegistry
    {
        public MvxIosBuilderRegistry(bool registerDefaultElements = true)
        {
            this.AddBuilder(typeof(IListLayout), new MvxIosListLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IListItemLayout), new MvxIosListItemLayoutBuilder(registerDefaultElements));
            this.AddBuilder(typeof(IMenu), new MvxIosMenuBuilder(registerDefaultElements));
        }
    }
}