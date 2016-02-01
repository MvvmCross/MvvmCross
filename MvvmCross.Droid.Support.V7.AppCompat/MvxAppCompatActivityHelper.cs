// MvxAppCompatActivityHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Content;
using Android.Util;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public static class MvxAppCompatActivityHelper
    {
        public static View OnCreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            // Swap our AppCompat Views
            switch (name)
            {
                case "MvxSpinner":
                    return new MvxAppCompatSpinner(context, attrs);
                case "MvxImageView":
                    return new MvxAppCompatImageView(context, attrs);
                case "MvxListView":
                    return new MvxAppCompatListView(context, attrs);
                case "MvxRadioGroup":
                    return new MvxAppCompatRadioGroup(context, attrs);
                case "MvxAutoCompleteTextView":
                    return new MvxAppCompatAutoCompleteTextView(context, attrs);
                default:
                    return null;
            }
        }
    }
}