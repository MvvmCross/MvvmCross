// MvxIosListLayoutBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Builders.Lists
{
    using CrossUI.Core.Builder;
    using CrossUI.Core.Elements.Lists;

    public class MvxIosListLayoutBuilder : TypedUserInterfaceBuilder
    {
        public MvxIosListLayoutBuilder(bool registerDefaults)
            : base(typeof(IListLayout), "ListLayout", "General")
        {
            if (registerDefaults)
            {
                this.RegisterConventionalKeys(this.GetType().Assembly);
            }
        }
    }
}