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
            => MvxIosPropertyBinding.UIControl_TouchUpInside;

        public static string BindValueChanged(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_ValueChanged;

        public static string BindVisibility(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Visibility;

        public static string BindVisible(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Visible;

        public static string BindHidden(this UIActivityIndicatorView uiActivityIndicatorView)
             => MvxIosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindHidden(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Hidden;

        public static string BindValue(this UISlider uiSlider)
            => MvxIosPropertyBinding.UISlider_Value;

        public static string BindValue(this UIStepper uiStepper)
            => MvxIosPropertyBinding.UIStepper_Value;

        public static string BindSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => MvxIosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindDate(this UIDatePicker uiDatePicker)
            => MvxIosPropertyBinding.UIDatePicker_Date;

        public static string BindShouldReturn(this UITextField uiTextField)
            => MvxIosPropertyBinding.UITextField_ShouldReturn;

        public static string BindTime(this UIDatePicker uiDatePicker)
            => MvxIosPropertyBinding.UIDatePicker_Time;

        public static string BindText(this UILabel uiLabel)
            => MvxIosPropertyBinding.UILabel_Text;

        public static string BindText(this UITextField uiTextField)
            => MvxIosPropertyBinding.UITextField_Text;

        public static string BindText(this UITextView uiTextView)
            => MvxIosPropertyBinding.UITextView_Text;

        public static string BindLayerBorderWidth(this UIView uiView)
            => MvxIosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindOn(this UISwitch uiSwitch)
            => MvxIosPropertyBinding.UISwitch_On;

        public static string BindText(this UISearchBar uiSearchBar)
            => MvxIosPropertyBinding.UISearchBar_Text;

        public static string BindTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_Title;

        public static string BindDisabledTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_DisabledTitle;

        public static string BindHighlightedTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindSelectedTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_SelectedTitle;

        public static string BindTap(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Tap;

        public static string BindDoubleTap(this UIView uiView)
            => MvxIosPropertyBinding.UIView_DoubleTap;

        public static string BindTwoFingerTap(this UIView uiView)
            => MvxIosPropertyBinding.UIView_TwoFingerTap;

        public static string BindTextFocusk(this UIView uiView)
            => MvxIosPropertyBinding.UITextField_TextFocus;
    }
}
