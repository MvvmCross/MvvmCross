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
        public static string BindingTouchUpInside(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_TouchUpInside;

        public static string BindingValueChanged(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_ValueChanged;

        public static string BindingVisibility(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Visibility;

        public static string BindingVisible(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Visible;

        public static string BindingHidden(this UIActivityIndicatorView uiActivityIndicatorView)
             => MvxIosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindingClick(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Hidden;

        public static string BindingValue(this UISlider uiSlider)
            => MvxIosPropertyBinding.UISlider_Value;

        public static string BindingValue(this UIStepper uiStepper)
            => MvxIosPropertyBinding.UIStepper_Value;

        public static string BindingSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => MvxIosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindingDate(this UIDatePicker uiDatePicker)
            => MvxIosPropertyBinding.UIDatePicker_Date;

        public static string BindingShouldReturn(this UITextField uiTextField)
            => MvxIosPropertyBinding.UITextField_ShouldReturn;

        public static string BindingTime(this UIDatePicker uiDatePicker)
            => MvxIosPropertyBinding.UIDatePicker_Time;

        public static string BindingText(this UILabel uiLabel)
            => MvxIosPropertyBinding.UILabel_Text;

        public static string BindingText(this UITextField uiTextField)
            => MvxIosPropertyBinding.UITextField_Text;

        public static string BindingText(this UITextView uiTextView)
            => MvxIosPropertyBinding.UITextView_Text;

        public static string BindingLayerBorderWidth(this UIView uiView)
            => MvxIosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindingOn(this UISwitch uiSwitch)
            => MvxIosPropertyBinding.UISwitch_On;

        public static string BindingText(this UISearchBar uiSearchBar)
            => MvxIosPropertyBinding.UISearchBar_Text;

        public static string BindingTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_Title;

        public static string BindingDisabledTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_DisabledTitle;

        public static string BindingHighlightedTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindingSelectedTitle(this UIButton uiButton)
            => MvxIosPropertyBinding.UIButton_SelectedTitle;

        public static string BindingTapk(this UIView uiView)
            => MvxIosPropertyBinding.UIView_Tap;

        public static string BindingDoubleTap(this UIView uiView)
            => MvxIosPropertyBinding.UIView_DoubleTap;

        public static string BindingTwoFingerTap(this UIView uiView)
            => MvxIosPropertyBinding.UIView_TwoFingerTap;

        public static string BindingTextFocusk(this UIView uiView)
            => MvxIosPropertyBinding.UITextField_TextFocus;
    }
}