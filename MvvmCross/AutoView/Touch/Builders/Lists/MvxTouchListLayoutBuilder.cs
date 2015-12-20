// MvxTouchListLayoutBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Builders.Lists
{
    using CrossUI.Core.Builder;
    using CrossUI.Core.Elements.Lists;

    public class MvxTouchListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxTouchListLayoutBuilder(bool registerDefaults)
            : base(typeof(IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                this.RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}