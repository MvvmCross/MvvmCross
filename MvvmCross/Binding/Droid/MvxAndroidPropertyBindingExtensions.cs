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
        public static string BindingClick(this View view) 
            => MvxAndroidPropertyBinding.View_Click;

        public static string BindingText(this TextView textview)
            => MvxAndroidPropertyBinding.TextView_Text;

        public static string BindingextFormatted(this TextView textview) 
            => MvxAndroidPropertyBinding.TextView_TextFormatted;

        public static string BindingHint(this TextView textview) 
            => MvxAndroidPropertyBinding.TextView_Hint;

        public static string BindingPartialText(this MvxAutoCompleteTextView mvxAutoCompleteTextView) 
            => MvxAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText;

        public static string BindingSelectedObject(this MvxAutoCompleteTextView mvxAutoCompleteTextView) 
            => MvxAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject;

        public static string BindingChecked(this CompoundButton compoundButton) 
            => MvxAndroidPropertyBinding.CompoundButton_Checked;

        public static string BindingProgress(this SeekBar seekBar) 
            => MvxAndroidPropertyBinding.SeekBar_Progress;

        public static string BindingVisible(this View view) 
            => MvxAndroidPropertyBinding.View_Visible;

        public static string BindingHidden(this View view)
            => MvxAndroidPropertyBinding.View_Hidden;

        public static string BindingBitmap(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_Bitmap;

        public static string BindingDrawable(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_Drawable;

        public static string BindingDrawableId(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_DrawableId;

        public static string BindingDrawableName(this ImageView imageView)
            => MvxAndroidPropertyBinding.ImageView_DrawableName;

        public static string BindingResourceName(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_ResourceName;

        public static string BindingAssetImagePath(this ImageView imageView) 
            => MvxAndroidPropertyBinding.ImageView_AssetImagePath;

        public static string BindingSelectedItem(this MvxSpinner mvxSpinner) 
            => MvxAndroidPropertyBinding.MvxSpinner_SelectedItem;

        public static string BindingSelectedItemPosition(this AdapterView adapterView) 
            => MvxAndroidPropertyBinding.AdapterView_SelectedItemPosition;

        public static string BindingSelectedItem(this MvxListView mvxListView) 
            => MvxAndroidPropertyBinding.MvxListView_SelectedItem;

        public static string BindingSelectedItem(this MvxExpandableListView mvxExpandableListView) 
            => MvxAndroidPropertyBinding.MvxExpandableListView_SelectedItem;

        public static string BindingRating(this RatingBar ratingBar) 
            => MvxAndroidPropertyBinding.RatingBar_Rating;

        public static string BindingLongClick(this View view) 
            => MvxAndroidPropertyBinding.View_LongClick;

        public static string BindingSelectedItem(this MvxRadioGroup mvxRadioGroup)
            => MvxAndroidPropertyBinding.MvxRadioGroup_SelectedItem;

        public static string BindingTextFocus(this EditText editText) 
            => MvxAndroidPropertyBinding.EditText_TextFocus;

        public static string BindingQuery(this SearchView searchView) 
            => MvxAndroidPropertyBinding.SearchView_Query;

        public static string BindingValue(this Preference preference) 
            => MvxAndroidPropertyBinding.Preference_Value;

        public static string BindingText(this EditTextPreference editTextPreference) 
            => MvxAndroidPropertyBinding.EditTextPreference_Text;

        public static string BindingValue(this ListPreference listPreference) 
            => MvxAndroidPropertyBinding.ListPreference_Value;

        public static string BindingChecked(this TwoStatePreference twoStatePreference) 
            => MvxAndroidPropertyBinding.TwoStatePreference_Checked;
    }
}