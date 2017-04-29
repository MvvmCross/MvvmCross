// MvxIosPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Binding.iOS
{
    public static class MvxIosPropertyBindingExtensions
    {
        public static string BindTouchUpInside(this UIControl uiControl)
        {
            return MvxIosPropertyBinding.UIControl_TouchUpInside;
        }

        public static string BindValueChanged(this UIControl uiControl)
        {
            return MvxIosPropertyBinding.UIControl_ValueChanged;
        }

        public static string BindVisibility(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_Visibility;
        }

        public static string BindVisible(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_Visible;
        }

        public static string BindHidden(this UIActivityIndicatorView uiActivityIndicatorView)
        {
            return MvxIosPropertyBinding.UIActivityIndicatorView_Hidden;
        }

        public static string BindHidden(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_Hidden;
        }

        public static string BindValue(this UISlider uiSlider)
        {
            return MvxIosPropertyBinding.UISlider_Value;
        }

        public static string BindValue(this UIStepper uiStepper)
        {
            return MvxIosPropertyBinding.UIStepper_Value;
        }

        public static string BindSelectedSegment(this UISegmentedControl uiSegmentedControl)
        {
            return MvxIosPropertyBinding.UISegmentedControl_SelectedSegment;
        }

        public static string BindDate(this UIDatePicker uiDatePicker)
        {
            return MvxIosPropertyBinding.UIDatePicker_Date;
        }

        public static string BindShouldReturn(this UITextField uiTextField)
        {
            return MvxIosPropertyBinding.UITextField_ShouldReturn;
        }

        public static string BindTime(this UIDatePicker uiDatePicker)
        {
            return MvxIosPropertyBinding.UIDatePicker_Time;
        }

        public static string BindText(this UILabel uiLabel)
        {
            return MvxIosPropertyBinding.UILabel_Text;
        }

        public static string BindText(this UITextField uiTextField)
        {
            return MvxIosPropertyBinding.UITextField_Text;
        }

        public static string BindText(this UITextView uiTextView)
        {
            return MvxIosPropertyBinding.UITextView_Text;
        }

        public static string BindLayerBorderWidth(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_LayerBorderWidth;
        }

        public static string BindOn(this UISwitch uiSwitch)
        {
            return MvxIosPropertyBinding.UISwitch_On;
        }

        public static string BindText(this UISearchBar uiSearchBar)
        {
            return MvxIosPropertyBinding.UISearchBar_Text;
        }

        public static string BindTitle(this UIButton uiButton)
        {
            return MvxIosPropertyBinding.UIButton_Title;
        }

        public static string BindDisabledTitle(this UIButton uiButton)
        {
            return MvxIosPropertyBinding.UIButton_DisabledTitle;
        }

        public static string BindHighlightedTitle(this UIButton uiButton)
        {
            return MvxIosPropertyBinding.UIButton_HighlightedTitle;
        }

        public static string BindSelectedTitle(this UIButton uiButton)
        {
            return MvxIosPropertyBinding.UIButton_SelectedTitle;
        }

        public static string BindTap(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_Tap;
        }

        public static string BindDoubleTap(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_DoubleTap;
        }

        public static string BindTwoFingerTap(this UIView uiView)
        {
            return MvxIosPropertyBinding.UIView_TwoFingerTap;
        }

        public static string BindTextFocusk(this UIView uiView)
        {
            return MvxIosPropertyBinding.UITextField_TextFocus;
        }
    }
}