// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

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