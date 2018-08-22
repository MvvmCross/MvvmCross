// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using UIKit;

namespace MvvmCross.Platforms.Ios.Binding
{
    public static class MvxIosPropertyBindingExtensions
    {
        public static string BindTouchUpInside(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_TouchUpInside;

        public static string BindValueChanged(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_ValueChanged;

        public static string BindTouchDown(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_TouchDown;

        public static string BindTouchDownRepeat(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_TouchDownRepeat;

        public static string BindTouchDragInside(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_TouchDragInside;

        public static string BindPrimaryActionTriggered(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_PrimaryActionTriggered;

        public static string BindEditingDidBegin(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_EditingDidBegin;

        public static string BindEditingChanged(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_EditingChanged;

        public static string BindEditingDidEnd(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_EditingDidEnd;

        public static string BindEditingDidEndOnExit(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_EditingDidEndOnExit;

        public static string BindAllTouchEvents(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_AllTouchEvents;

        public static string BindAllEditingEvents(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_AllEditingEvents;

        public static string BindAllEvents(this UIControl uiControl)
            => MvxIosPropertyBinding.UIControl_AllEvents;

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
        
        public static string BindCountDownDuration(this UIDatePicker uiDatePicker)
            => MvxIosPropertyBinding.UIDatePicker_CountDownDuration;

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

        public static string BindTextFocus(this UIView uiView)
            => MvxIosPropertyBinding.UITextField_TextFocus;

        public static string BindClicked(this UIBarButtonItem uiBarButtonItem)
            => MvxIosPropertyBinding.UIBarButtonItem_Clicked;
    }
}
