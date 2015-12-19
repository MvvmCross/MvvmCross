// IMvxLayoutListItemViewFactory.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.Droid.Interfaces.Lists
{
    using Android.Content;
    using Android.Views;

    using CrossUI.Core.Elements.Lists;

    using MvvmCross.Binding.Droid.BindingContext;

    public interface IMvxLayoutListItemViewFactory
        : IListItemLayout
    {
        string UniqueName { get; }

        View BuildView(Context context, IMvxAndroidBindingContext androidBindingContext, object source);
    }
}