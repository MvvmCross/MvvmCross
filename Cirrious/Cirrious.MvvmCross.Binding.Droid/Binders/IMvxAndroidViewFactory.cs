// IMvxAndroidViewFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Util;
using Android.Views;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public interface IMvxAndroidViewFactory
    {
        View CreateView(string name, Context context, IAttributeSet attrs);
    }
}