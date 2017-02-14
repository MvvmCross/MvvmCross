// MvxTvosPropertyBindingExtensions.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using UIKit;

namespace MvvmCross.Binding.tvOS
{
    public static class MvxTvosPropertyBindingExtensions
    {
        public static string BindingTouchUpInside(this UIControl uiControl)
            => MvxTvosPropertyBinding.UIControl_TouchUpInside;

        public static string BindingValueChanged(this UIControl uiControl)
            => MvxTvosPropertyBinding.UIControl_ValueChanged;

        public static string BindingVisibility(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Visibility;

        public static string BindingVisible(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Visible;

        public static string BindingHidden(this UIActivityIndicatorView uiActivityIndicatorView)
            => MvxTvosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindingHidden(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Hidden;

        public static string BindingSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => MvxTvosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindingShouldReturn(this UITextField uiTextField)
            => MvxTvosPropertyBinding.UITextField_ShouldReturn;

        public static string BindingText(this UILabel uiLabel)
            => MvxTvosPropertyBinding.UILabel_Text;

        public static string BindingText(this UITextField uiTextField)
            => MvxTvosPropertyBinding.UITextField_Text;

        public static string BindingText(this UITextView uiTextView)
            => MvxTvosPropertyBinding.UITextView_Text;

        public static string BindingLayerBorderWidth(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindingText(this UISearchBar uiSearchBar)
            => MvxTvosPropertyBinding.UISearchBar_Text;

        public static string BindingTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_Title;

        public static string BindingDisabledTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_DisabledTitle;

        public static string BindingHighlightedTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindingSelectedTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_SelectedTitle;

        public static string BindingTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Tap;

        public static string BindingDoubleTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_DoubleTap;

        public static string BindingTwoFingerTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_TwoFingerTap;

        public static string BindingTextFocusk(this UIView uiView)
            => MvxTvosPropertyBinding.UITextField_TextFocus;
    }
}
