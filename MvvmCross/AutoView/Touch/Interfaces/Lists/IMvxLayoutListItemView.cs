// IMvxLayoutListItemView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Touch.Interfaces.Lists
{
    public interface IMvxLayoutListItemView
    {
        string UniqueName { get; }

        void BindTo(object source);
    }
}