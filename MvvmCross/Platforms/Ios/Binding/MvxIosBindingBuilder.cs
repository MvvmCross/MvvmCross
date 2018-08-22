// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Converters;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Ios.Binding.Target;
using MvvmCross.Platforms.Ios.Binding.ValueConverters;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding
{
    public class MvxIosBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<IMvxAutoValueConverters> _fillAutoValueConvertersAction;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;
        private readonly MvxUnifiedTypesValueConverter _unifiedValueTypesConverter;

        public MvxIosBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
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
                MvxIosPropertyBinding.UIControl_TouchDown,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_TouchDown));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_TouchDownRepeat,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_TouchDownRepeat));
            
            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_TouchDragInside,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_TouchDragInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_TouchUpInside,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_TouchUpInside));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_ValueChanged,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_ValueChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_PrimaryActionTriggered,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_PrimaryActionTriggered));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_EditingDidBegin,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_EditingDidBegin));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_EditingChanged,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_EditingChanged));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_EditingDidEnd,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_EditingDidEnd));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_EditingDidEndOnExit,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_EditingDidEndOnExit));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_AllTouchEvents,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_AllTouchEvents));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_AllEditingEvents,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_AllEditingEvents));
            
            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_AllEvents,
                view => new MvxUIControlTargetBinding(view, MvxIosPropertyBinding.UIControl_AllEvents));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_Visibility,
                view => new MvxUIViewVisibilityTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_Visible,
                view =>   new MvxUIViewVisibleTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>(
                MvxIosPropertyBinding.UIActivityIndicatorView_Hidden,
                activityIndicator => new MvxUIActivityIndicatorViewHiddenTargetBinding(activityIndicator));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_Hidden,
                view => new MvxUIViewHiddenTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISliderValueTargetBinding),
                typeof(UISlider),
                MvxIosPropertyBinding.UISlider_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIStepperValueTargetBinding),
                typeof(UIStepper),
                MvxIosPropertyBinding.UIStepper_Value);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIPageControlCurrentPageTargetBinding),
                typeof(UIPageControl),
                MvxIosPropertyBinding.UIPageControl_CurrentPage);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISegmentedControlSelectedSegmentTargetBinding),
                typeof(UISegmentedControl),
                MvxIosPropertyBinding.UISegmentedControl_SelectedSegment);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerDateTargetBinding),
                typeof(UIDatePicker),
                MvxIosPropertyBinding.UIDatePicker_Date);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                MvxIosPropertyBinding.UIDatePicker_MinimumDate);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerMinMaxTargetBinding),
                typeof(UIDatePicker),
                MvxIosPropertyBinding.UIDatePicker_MaximumDate);

            registry.RegisterCustomBindingFactory<UIDatePicker>(
                MvxIosPropertyBinding.UIDatePicker_Time,
                view => new MvxUIDatePickerTimeTargetBinding(view, typeof(UIDatePicker).GetProperty(MvxIosPropertyBinding.UIDatePicker_Date)));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerCountDownDurationTargetBinding),
                typeof(UIDatePicker),
                MvxIosPropertyBinding.UIDatePicker_CountDownDuration);

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxIosPropertyBinding.UITextField_ShouldReturn,
                textField => new MvxUITextFieldShouldReturnTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UILabel>(
                MvxIosPropertyBinding.UILabel_Text,
                view => new MvxUILabelTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxIosPropertyBinding.UITextField_Text,
                view => new MvxUITextFieldTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UITextView>(
                MvxIosPropertyBinding.UITextView_Text,
                view => new MvxUITextViewTextTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_LayerBorderWidth,
                view => new MvxUIViewLayerBorderWidthTargetBinding(view));

            registry.RegisterCustomBindingFactory<UISwitch>(
                MvxIosPropertyBinding.UISwitch_On,
                uiSwitch => new MvxUISwitchOnTargetBinding(uiSwitch));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISearchBarTextTargetBinding),
                typeof(UISearchBar),
                MvxIosPropertyBinding.UISearchBar_Text);

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxIosPropertyBinding.UIButton_Title,
                button => new MvxUIButtonTitleTargetBinding(button));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxIosPropertyBinding.UIButton_DisabledTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Disabled));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxIosPropertyBinding.UIButton_HighlightedTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Highlighted));

            registry.RegisterCustomBindingFactory<UIButton>(
                MvxIosPropertyBinding.UIButton_SelectedTitle,
                button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Selected));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_Tap,
                view => new MvxUIViewTapTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_DoubleTap,
                view => new MvxUIViewTapTargetBinding(view, 2, 1));

            registry.RegisterCustomBindingFactory<UIView>(
                MvxIosPropertyBinding.UIView_TwoFingerTap,
                view => new MvxUIViewTapTargetBinding(view, 1, 2));

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxIosPropertyBinding.UITextField_TextFocus,
                textField => new MvxUITextFieldTextFocusTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UIBarButtonItem>(
                MvxIosPropertyBinding.UIBarButtonItem_Clicked,
                view => new MvxUIBarButtonItemTargetBinding(view));

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

            registry.AddOrOverwrite(typeof(UIButton), MvxIosPropertyBinding.UIControl_TouchUpInside);
            registry.AddOrOverwrite(typeof(UIBarButtonItem), nameof(UIBarButtonItem.Clicked));
            registry.AddOrOverwrite(typeof(UISearchBar), MvxIosPropertyBinding.UISearchBar_Text);
            registry.AddOrOverwrite(typeof(UITextField), MvxIosPropertyBinding.UITextField_Text);
            registry.AddOrOverwrite(typeof(UITextView), MvxIosPropertyBinding.UITextView_Text);
            registry.AddOrOverwrite(typeof(UILabel), MvxIosPropertyBinding.UILabel_Text);
            registry.AddOrOverwrite(typeof(MvxCollectionViewSource), nameof(MvxCollectionViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxTableViewSource), nameof(MvxTableViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIDatePicker), MvxIosPropertyBinding.UIDatePicker_Date);
            registry.AddOrOverwrite(typeof(UISlider), MvxIosPropertyBinding.UISlider_Value);
            registry.AddOrOverwrite(typeof(UISwitch), MvxIosPropertyBinding.UISwitch_On);
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(UISegmentedControl), MvxIosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), MvxIosPropertyBinding.UIActivityIndicatorView_Hidden);

            _fillBindingNamesAction?.Invoke(registry);
        }
    }
}
