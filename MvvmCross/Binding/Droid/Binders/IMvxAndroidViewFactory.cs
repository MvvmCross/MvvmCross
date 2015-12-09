// IMvxAndroidViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders
{
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface IMvxAndroidViewFactory
    {
        View CreateView(View parent, string name, Context context, IAttributeSet attrs);
    }
}