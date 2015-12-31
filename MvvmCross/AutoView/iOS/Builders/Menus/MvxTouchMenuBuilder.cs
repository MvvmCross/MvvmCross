// MvxTouchMenuBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders.Menus
{
    using CrossUI.Core.Builder;
    using CrossUI.Core.Elements.Menu;

    public class MvxTouchMenuBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchMenuBuilder(bool registerDefaults)
            : base(typeof(IMenu), "Menu", "CaptionAndIcon")
        {
            if (registerDefaults)
            {
                this.RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}