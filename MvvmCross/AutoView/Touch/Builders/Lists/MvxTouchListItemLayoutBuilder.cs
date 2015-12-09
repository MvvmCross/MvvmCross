// MvxTouchListItemLayoutBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Builders.Lists
{
    using CrossUI.Core.Builder;

    using MvvmCross.AutoView.Touch.Interfaces.Lists;

    public class MvxTouchListItemLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListItemLayoutBuilder(bool registerDefaults)
            : base(typeof(IMvxLayoutListItemViewFactory), "ListItemViewFactory", "General")
        {
            if (registerDefaults)
            {
                this.RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}