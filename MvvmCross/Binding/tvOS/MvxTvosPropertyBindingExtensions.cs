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
        public static string BindTouchUpInside(this UIControl uiControl)
            => MvxTvosPropertyBinding.UIControl_TouchUpInside;

        public static string BindValueChanged(this UIControl uiControl)
            => MvxTvosPropertyBinding.UIControl_ValueChanged;

        public static string BindVisibility(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Visibility;

        public static string BindVisible(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Visible;

        public static string BindHidden(this UIActivityIndicatorView uiActivityIndicatorView)
            => MvxTvosPropertyBinding.UIActivityIndicatorView_Hidden;

        public static string BindHidden(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Hidden;

        public static string BindSelectedSegment(this UISegmentedControl uiSegmentedControl)
            => MvxTvosPropertyBinding.UISegmentedControl_SelectedSegment;

        public static string BindShouldReturn(this UITextField uiTextField)
            => MvxTvosPropertyBinding.UITextField_ShouldReturn;

        public static string BindText(this UILabel uiLabel)
            => MvxTvosPropertyBinding.UILabel_Text;

        public static string BindText(this UITextField uiTextField)
            => MvxTvosPropertyBinding.UITextField_Text;

        public static string BindText(this UITextView uiTextView)
            => MvxTvosPropertyBinding.UITextView_Text;

        public static string BindLayerBorderWidth(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_LayerBorderWidth;

        public static string BindText(this UISearchBar uiSearchBar)
            => MvxTvosPropertyBinding.UISearchBar_Text;

        public static string BindTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_Title;

        public static string BindDisabledTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_DisabledTitle;

        public static string BindHighlightedTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_HighlightedTitle;

        public static string BindSelectedTitle(this UIButton uiButton)
            => MvxTvosPropertyBinding.UIButton_SelectedTitle;

        public static string BindTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_Tap;

        public static string BindDoubleTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_DoubleTap;

        public static string BindTwoFingerTap(this UIView uiView)
            => MvxTvosPropertyBinding.UIView_TwoFingerTap;

        public static string BindTextFocusk(this UIView uiView)
            => MvxTvosPropertyBinding.UITextField_TextFocus;
    }
}
