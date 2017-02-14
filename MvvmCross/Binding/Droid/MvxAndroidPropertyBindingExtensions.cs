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
            => MvxAndroidPropertyBinding.View_Click;

        public static string BindText(this TextView textview)
            => MvxAndroidPropertyBinding.TextView_Text;

        public static string BindextFormatted(this TextView textview) 
            => MvxAndroidPropertyBinding.TextView_TextFormatted;

        public static string BindHint(this TextView textview) 
            => MvxAndroidPropertyBinding.TextView_Hint;

        public static string BindPartialText(this MvxAutoCompleteTextView mvxAutoCompleteTextView) 
            => MvxAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText;

        public static string BindSelectedObject(this MvxAutoCompleteTextView mvxAutoCompleteTextView) 
            => MvxAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject;

        public static string BindChecked(this CompoundButton compoundButton) 
            => MvxAndroidPropertyBinding.CompoundButton_Checked;

        public static string BindProgress(this SeekBar seekBar) 
            => MvxAndroidPropertyBinding.SeekBar_Progress;

        public static string BindVisible(this View view) 
            => MvxAndroidPropertyBinding.View_Visible;

        public static string BindHidden(this View view)
            => MvxAndroidPropertyBinding.View_Hidden;

        public static string BindBitmap(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_Bitmap;

        public static string BindDrawable(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_Drawable;

        public static string BindDrawableId(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_DrawableId;

        public static string BindDrawableName(this ImageView imageView)
            => MvxAndroidPropertyBinding.ImageView_DrawableName;

        public static string BindResourceName(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_ResourceName;

        public static string BindAssetImagePath(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_AssetImagePath;

        public static string BindSelectedItem(this MvxSpinner mvxSpinner) 
            => MvxAndroidPropertyBinding.MvxSpinner_SelectedItem;

        public static string BindSelectedItemPosition(this AdapterView adapterView) 
            => MvxAndroidPropertyBinding.AdapterView_SelectedItemPosition;

        public static string BindSelectedItem(this MvxListView mvxListView) 
            => MvxAndroidPropertyBinding.MvxListView_SelectedItem;

        public static string BindSelectedItem(this MvxExpandableListView mvxExpandableListView) 
            => MvxAndroidPropertyBinding.MvxExpandableListView_SelectedItem;

        public static string BindRating(this RatingBar ratingBar) 
            => MvxAndroidPropertyBinding.RatingBar_Rating;

        public static string BindLongClick(this View view) 
            => MvxAndroidPropertyBinding.View_LongClick;

        public static string BindSelectedItem(this MvxRadioGroup mvxRadioGroup)
            => MvxAndroidPropertyBinding.MvxRadioGroup_SelectedItem;

        public static string BindTextFocus(this EditText editText) 
            => MvxAndroidPropertyBinding.EditText_TextFocus;

        public static string BindQuery(this SearchView searchView) 
            => MvxAndroidPropertyBinding.SearchView_Query;

        public static string BindValue(this Preference preference) 
            => MvxAndroidPropertyBinding.Preference_Value;

        public static string BindText(this EditTextPreference editTextPreference) 
            => MvxAndroidPropertyBinding.EditTextPreference_Text;

        public static string BindValue(this ListPreference listPreference) 
            => MvxAndroidPropertyBinding.ListPreference_Value;

        public static string BindChecked(this TwoStatePreference twoStatePreference) 
            => MvxAndroidPropertyBinding.TwoStatePreference_Checked;
    }
}