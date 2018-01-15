// MvxAppCompatPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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
