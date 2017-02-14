// MvxMacPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using AppKit;

namespace MvvmCross.Binding.Mac
{
    public static class MvxMacPropertyBindingExtensions
    {
        public static string BindingVisibility(this NSView nsView)
            => MvxMacPropertyBinding.NSView_Visibility;

        public static string BindingVisible(this NSView nsView)
            => MvxMacPropertyBinding.NSView_Visible;

        public static string BindingIntValue(this NSSlider nsSlider)
            => MvxMacPropertyBinding.NSSlider_IntValue;

        public static string BindingSelectedSegment(this NSSegmentedControl nsSegmentedControl)
            => MvxMacPropertyBinding.NSSegmentedControl_SelectedSegment;

        public static string BindingTime(this NSDatePicker nsDatePicker)
            => MvxMacPropertyBinding.NSDatePicker_Time;

        public static string BindingDate(this NSDatePicker nsDatePicker)
            => MvxMacPropertyBinding.NSDatePicker_Date;

        public static string BindingStringValue(this NSTextField nsTextField)
            => MvxMacPropertyBinding.NSTextField_StringValue;

        public static string BindingStringValue(this NSTextView nsTextView)
            => MvxMacPropertyBinding.NSTextView_StringValue;

        public static string BindingState(this NSButton nsButton)
            => MvxMacPropertyBinding.NSButton_State;

        public static string BindingText(this NSSearchField nsSearchField)
            => MvxMacPropertyBinding.NSSearchField_Text;

        public static string BindingTitle(this NSButton nsButton)
            => MvxMacPropertyBinding.NSButton_Title;
    }
}
