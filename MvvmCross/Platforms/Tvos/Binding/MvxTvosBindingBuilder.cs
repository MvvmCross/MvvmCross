// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Base;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Converters;
using MvvmCross.Platforms.Tvos.Binding.Target;
using MvvmCross.Platforms.Tvos.Binding.ValueConverters;
using MvvmCross.Platforms.Tvos.Binding.Views;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding
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
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillAutoValueConvertersAction = fillAutoValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;

            _unifiedValueTypesConverter = new MvxUnifiedTypesValueConverter();
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

            _fillRegistryAction?.Invoke(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            _fillValueConvertersAction?.Invoke(registry);
        }

        protected override void FillAutoValueConverters(IMvxAutoValueConverters autoValueConverters)
        {
            base.FillAutoValueConverters(autoValueConverters);

            //register converter for xamarin unified types
            foreach (var kvp in MvxUnifiedTypesValueConverter.UnifiedTypeConversions)
                autoValueConverters.Register(kvp.Key, kvp.Value, _unifiedValueTypesConverter);

            _fillAutoValueConvertersAction?.Invoke(autoValueConverters);
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
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(UISegmentedControl), MvxTvosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), MvxTvosPropertyBinding.UIActivityIndicatorView_Hidden);

            _fillBindingNamesAction?.Invoke(registry);
        }
    }
}
