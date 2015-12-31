// MvxIosListItemLayoutBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders.Lists
{
    using CrossUI.Core.Builder;

    using MvvmCross.AutoView.iOS.Interfaces.Lists;

    public class MvxIosListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxIosListItemLayoutBuilder(bool registerDefaults)
            : base(typeof(IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                this.RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}