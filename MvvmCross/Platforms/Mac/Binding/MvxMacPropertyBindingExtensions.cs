// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;

namespace MvvmCross.Platforms.Mac.Binding
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

        public static string BindSelectedTag(this NSPopUpButton nsPopUpButton)
            => MvxMacPropertyBinding.NSPopUpButton_SelectedTag;

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

        public static string BindState(this NSMenuItem nsMenuItem)
            => MvxMacPropertyBinding.NSMenuItem_State;

        public static string BindText(this NSSearchField nsSearchField)
            => MvxMacPropertyBinding.NSSearchField_Text;

        public static string BindTitle(this NSButton nsButton)
            => MvxMacPropertyBinding.NSButton_Title;

        public static string BindSelectedTabViewItemIndex(this NSTabViewController nsTabViewController)
            => MvxMacPropertyBinding.NSTabViewController_SelectedTabViewItemIndex;
    }
}
