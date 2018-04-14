// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Support.V7.Widget;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public static class MvxAppCompatPropertyBindingExtensions
    {
        public static string BindPartialText(this MvxAppCompatAutoCompleteTextView mvxAppCompatAutoCompleteTextView)
            => MvxAppCompatPropertyBinding.MvxAppCompatAutoCompleteTextView_PartialText;

        public static string BindSelectedObject(this MvxAppCompatAutoCompleteTextView mvxAppCompatAutoCompleteTextView)
            => MvxAppCompatPropertyBinding.MvxAppCompatAutoCompleteTextView_SelectedObject;

        public static string BindSelectedItem(this MvxAppCompatSpinner mvxAppCompatSpinner)
            => MvxAppCompatPropertyBinding.MvxAppCompatSpinner_SelectedItem;

        public static string BindSelectedItem(this MvxAppCompatRadioGroup mvxAppCompatRadioGroup)
            => MvxAppCompatPropertyBinding.MvxAppCompatRadioGroup_SelectedItem;

        public static string BindQuery(this SearchView searchView)
            => MvxAppCompatPropertyBinding.SearchView_Query;

        public static string BindSubtitle(this Toolbar toolbar)
           => MvxAppCompatPropertyBinding.Toolbar_Subtitle;
    }
}
