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
        public static string BindVisibility(this NSView nsView)
            => MvxMacPropertyBinding.NSView_Visibility;

        public static string BindVisible(this NSView nsView)
            => MvxMacPropertyBinding.NSView_Visible;

        public static string BindIntValue(this NSSlider nsSlider)
            => MvxMacPropertyBinding.NSSlider_IntValue;

        public static string BindSelectedSegment(this NSSegmentedControl nsSegmentedControl)
            => MvxMacPropertyBinding.NSSegmentedControl_SelectedSegment;

        public static string BindTime(this NSDatePicker nsDatePicker)
            => MvxMacPropertyBinding.NSDatePicker_Time;

        public static string BindDate(this NSDatePicker nsDatePicker)
            => MvxMacPropertyBinding.NSDatePicker_Date;

        public static string BindStringValue(this NSTextField nsTextField)
            => MvxMacPropertyBinding.NSTextField_StringValue;

        public static string BindStringValue(this NSTextView nsTextView)
            => MvxMacPropertyBinding.NSTextView_StringValue;

        public static string BindState(this NSButton nsButton)
            => MvxMacPropertyBinding.NSButton_State;

        public static string BindText(this NSSearchField nsSearchField)
            => MvxMacPropertyBinding.NSSearchField_Text;

        public static string BindTitle(this NSButton nsButton)
            => MvxMacPropertyBinding.NSButton_Title;
    }
}
