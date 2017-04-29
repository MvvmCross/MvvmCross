// MvxAndroidPropertyBinding.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Preferences;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace MvvmCross.Binding.Droid
{
    public static class MvxAndroidPropertyBindingExtensions
    {
        public static string BindClick(this View view)
        {
            return MvxAndroidPropertyBinding.View_Click;
        }

        public static string BindText(this TextView textview)
        {
            return MvxAndroidPropertyBinding.TextView_Text;
        }

        public static string BindTextFormatted(this TextView textview)
        {
            return MvxAndroidPropertyBinding.TextView_TextFormatted;
        }

        public static string BindHint(this TextView textview)
        {
            return MvxAndroidPropertyBinding.TextView_Hint;
        }

        public static string BindPartialText(this MvxAutoCompleteTextView mvxAutoCompleteTextView)
        {
            return MvxAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText;
        }

        public static string BindSelectedObject(this MvxAutoCompleteTextView mvxAutoCompleteTextView)
        {
            return MvxAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject;
        }

        public static string BindChecked(this CompoundButton compoundButton)
        {
            return MvxAndroidPropertyBinding.CompoundButton_Checked;
        }

        public static string BindProgress(this SeekBar seekBar)
        {
            return MvxAndroidPropertyBinding.SeekBar_Progress;
        }

        public static string BindVisible(this View view)
        {
            return MvxAndroidPropertyBinding.View_Visible;
        }

        public static string BindHidden(this View view)
        {
            return MvxAndroidPropertyBinding.View_Hidden;
        }

        public static string BindBitmap(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_Bitmap;
        }

        public static string BindDrawable(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_Drawable;
        }

        public static string BindDrawableId(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_DrawableId;
        }

        public static string BindDrawableName(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_DrawableName;
        }

        public static string BindResourceName(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_ResourceName;
        }

        public static string BindAssetImagePath(this ImageView imageView)
        {
            return MvxAndroidPropertyBinding.ImageView_AssetImagePath;
        }

        public static string BindSelectedItem(this MvxSpinner mvxSpinner)
        {
            return MvxAndroidPropertyBinding.MvxSpinner_SelectedItem;
        }

        public static string BindSelectedItemPosition(this AdapterView adapterView)
        {
            return MvxAndroidPropertyBinding.AdapterView_SelectedItemPosition;
        }

        public static string BindSelectedItem(this MvxListView mvxListView)
        {
            return MvxAndroidPropertyBinding.MvxListView_SelectedItem;
        }

        public static string BindSelectedItem(this MvxExpandableListView mvxExpandableListView)
        {
            return MvxAndroidPropertyBinding.MvxExpandableListView_SelectedItem;
        }

        public static string BindRating(this RatingBar ratingBar)
        {
            return MvxAndroidPropertyBinding.RatingBar_Rating;
        }

        public static string BindLongClick(this View view)
        {
            return MvxAndroidPropertyBinding.View_LongClick;
        }

        public static string BindSelectedItem(this MvxRadioGroup mvxRadioGroup)
        {
            return MvxAndroidPropertyBinding.MvxRadioGroup_SelectedItem;
        }

        public static string BindTextFocus(this EditText editText)
        {
            return MvxAndroidPropertyBinding.EditText_TextFocus;
        }

        public static string BindQuery(this SearchView searchView)
        {
            return MvxAndroidPropertyBinding.SearchView_Query;
        }

        public static string BindValue(this Preference preference)
        {
            return MvxAndroidPropertyBinding.Preference_Value;
        }

        public static string BindText(this EditTextPreference editTextPreference)
        {
            return MvxAndroidPropertyBinding.EditTextPreference_Text;
        }

        public static string BindValue(this ListPreference listPreference)
        {
            return MvxAndroidPropertyBinding.ListPreference_Value;
        }

        public static string BindChecked(this TwoStatePreference twoStatePreference)
        {
            return MvxAndroidPropertyBinding.TwoStatePreference_Checked;
        }
    }
}