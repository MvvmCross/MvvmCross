using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public class MvxAppCompatViewFactory : MvxAndroidViewFactory
    {
        public override View CreateView(View parent, string name, Context context, IAttributeSet attrs)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                // If we're running pre-Lollipop, we need to 'inject' our tint aware Views in place of the standard framework versions
                switch (name)
                {
                    case "EditText":
                        return new AppCompatEditText(context, attrs);
                    case "Spinner":
                        return new AppCompatSpinner(context, attrs);
                    case "MvxSpinner":
                    case "Mvx.MvxSpinner":
                        return new MvxAppCompatSpinner(context, attrs);
                    case "CheckBox":
                        return new AppCompatCheckBox(context, attrs);
                    case "RadioButton":
                        return new AppCompatRadioButton(context, attrs);
                    case "CheckedTextView":
                        return new AppCompatCheckedTextView(context, attrs);
                    case "AutoCompleteTextView":
                        return new AppCompatAutoCompleteTextView(context, attrs);
                    case "MultiAutoCompleteTextView":
                        return new AppCompatMultiAutoCompleteTextView(context, attrs);
                    case "RatingBar":
                        return new AppCompatRatingBar(context, attrs);
                    case "Button":
                        return new AppCompatButton(context, attrs);
                    case "Switch":
                        return new SwitchCompat(context, attrs);

                }
            }
            return base.CreateView(parent, name, context, attrs);
        }
    }
}