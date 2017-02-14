// MvxTvosBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.tvOS.Target;
using MvvmCross.Binding.tvOS.ValueConverters;
using MvvmCross.Binding.tvOS.Views;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.tvOS
{
    public class MvxTvosBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<IMvxAutoValueConverters> _fillAutoValueConvertersAction;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;
        private readonly MvxUnifiedTypesValueConverter _unifiedValueTypesConverter;

        public MvxTvosBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
                                    Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
                                    Action<IMvxAutoValueConverters> fillAutoValueConvertersAction = null,
                                    Action<IMvxBindingNameRegistry> fillBindingNamesAction = null)
        {
            this._fillRegistryAction = fillRegistryAction;
            this._fillValueConvertersAction = fillValueConvertersAction;
            this._fillAutoValueConvertersAction = fillAutoValueConvertersAction;
            this._fillBindingNamesAction = fillBindingNamesAction;

            this._unifiedValueTypesConverter = new MvxUnifiedTypesValueConverter();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxTvosPropertyBinding.UIControl_TouchUpInside,
                view => new MvxUIControlTouchUpInsideTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxTvosPropertyBinding.UIControl_ValueChanged,
                view => new MvxUIControlValueChangedTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_Visibility,
                view => new MvxUIViewVisibilityTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_Visible,
                view => new MvxUIViewVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>(
                MvxTvosPropertyBinding.UIActivityIndicatorView_Hidden,
                activityIndicator => new MvxUIActivityIndicatorViewHiddenTargetBinding(activityIndicator));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_Hidden,
                view => new MvxUIViewHiddenTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISegmentedControlSelectedSegmentTargetBinding),
                typeof(UISegmentedControl),
                MvxTvosPropertyBinding.UISegmentedControl_SelectedSegment);

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxTvosPropertyBinding.UITextField_ShouldReturn,
                textField => new MvxUITextFieldShouldReturnTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UILabel>(
                MvxTvosPropertyBinding.UILabel_Text,
                view => new MvxUILabelTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxTvosPropertyBinding.UITextField_Text,
                view => new MvxUITextFieldTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextView>(
                MvxTvosPropertyBinding.UITextView_Text,
                view => new MvxUITextViewTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_LayerBorderWidth,
                view => new MvxUIViewLayerBorderWidthTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISearchBarTextTargetBinding),
                typeof(UISearchBar),
                MvxTvosPropertyBinding.UISearchBar_Text);

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxTvosPropertyBinding.UIButton_Title,
                button => new MvxUIButtonTitleTargetBinding(button));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxTvosPropertyBinding.UIButton_DisabledTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Disabled));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxTvosPropertyBinding.UIButton_HighlightedTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Highlighted));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxTvosPropertyBinding.UIButton_SelectedTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Selected));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_Tap,
                view => new MvxUIViewTapTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_DoubleTap,
                view => new MvxUIViewTapTargetBinding(view, 2, 1));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxTvosPropertyBinding.UIView_TwoFingerTap,
                view => new MvxUIViewTapTargetBinding(view, 1, 2));

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxTvosPropertyBinding.UITextField_TextFocus,
                textField => new MvxUITextFieldTextFocusTargetBinding(textField));

            /*
            registry.RegisterCustomBindingFactory<UIView>("TwoFingerDoubleTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 2, 2));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 1, 3));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerDoubleTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 3, 3));
            */

            this._fillRegistryAction?.Invoke(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            this._fillValueConvertersAction?.Invoke(registry);
        }

        protected override void FillAutoValueConverters(IMvxAutoValueConverters autoValueConverters)
        {
            base.FillAutoValueConverters(autoValueConverters);

            //register converter for xamarin unified types
            foreach (var kvp in MvxUnifiedTypesValueConverter.UnifiedTypeConversions)
                autoValueConverters.Register(kvp.Key, kvp.Value, this._unifiedValueTypesConverter);

            this._fillAutoValueConvertersAction?.Invoke(autoValueConverters);
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(UIButton), MvxTvosPropertyBinding.UIControl_TouchUpInside);
            registry.AddOrOverwrite(typeof(UIBarButtonItem), nameof(UIBarButtonItem.Clicked));

            registry.AddOrOverwrite(typeof(UISearchBar), MvxTvosPropertyBinding.UISearchBar_Text);
            registry.AddOrOverwrite(typeof(UITextField), MvxTvosPropertyBinding.UITextField_Text);
            registry.AddOrOverwrite(typeof(UITextView), MvxTvosPropertyBinding.UITextView_Text);
            registry.AddOrOverwrite(typeof(UILabel), MvxTvosPropertyBinding.UILabel_Text);
            registry.AddOrOverwrite(typeof(MvxCollectionViewSource), nameof(MvxCollectionViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxTableViewSource), nameof(MvxTableViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxImageView), nameof(MvxImageView.ImageUrl));
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(IMvxImageHelper<UIImage>), nameof(IMvxImageHelper<UIImage>.ImageUrl));
            registry.AddOrOverwrite(typeof(MvxImageViewLoader), nameof(MvxImageViewLoader.ImageUrl));
            registry.AddOrOverwrite(typeof(UISegmentedControl), MvxTvosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), MvxTvosPropertyBinding.UIActivityIndicatorView_Hidden);

            this._fillBindingNamesAction?.Invoke(registry);
        }
    }
}