// MvxIosBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.iOS.Target;
using MvvmCross.Binding.iOS.ValueConverters;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace MvvmCross.Binding.iOS
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
                MvxIosPropertyBinding.UIControl_TouchUpInside,
                view => new MvxUIControlTouchUpInsideTargetBinding(view));

            registry.RegisterCustomBindingFactory<UIControl>(
                MvxIosPropertyBinding.UIControl_ValueChanged,
                view => new MvxUIControlValueChangedTargetBinding(view));

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
                typeof(MvxUISegmentedControlSelectedSegmentTargetBinding),
                typeof(UISegmentedControl),
                MvxIosPropertyBinding.UISegmentedControl_SelectedSegment);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUIDatePickerDateTargetBinding),
                typeof(UIDatePicker),
                MvxIosPropertyBinding.UIDatePicker_Date);

            registry.RegisterCustomBindingFactory<UITextField>(
                MvxIosPropertyBinding.UITextField_ShouldReturn,
                textField => new MvxUITextFieldShouldReturnTargetBinding(textField));

            registry.RegisterCustomBindingFactory<UIDatePicker>(
                MvxIosPropertyBinding.UIDatePicker_Time,
                view => new MvxUIDatePickerTimeTargetBinding(view, (typeof(UIDatePicker).GetProperty(nameof(UIDatePicker.Date)))));

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

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxUISwitchOnTargetBinding),
                typeof(UISwitch),
                MvxIosPropertyBinding.UISwitch_On);

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

            registry.AddOrOverwrite(typeof(UIButton), MvxIosPropertyBinding.UIControl_TouchUpInside);
            registry.AddOrOverwrite(typeof(UIBarButtonItem), nameof(UIBarButtonItem.Clicked));
            registry.AddOrOverwrite(typeof(UISearchBar), MvxIosPropertyBinding.UISearchBar_Text);
            registry.AddOrOverwrite(typeof(UITextField), MvxIosPropertyBinding.UITextField_Text);
            registry.AddOrOverwrite(typeof(UITextView), MvxIosPropertyBinding.UITextView_Text);
            registry.AddOrOverwrite(typeof(UILabel), MvxIosPropertyBinding.UILabel_Text);
            registry.AddOrOverwrite(typeof(MvxCollectionViewSource), nameof(MvxCollectionViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxTableViewSource), nameof(MvxTableViewSource.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxImageView), nameof(MvxImageView.ImageUrl));
            registry.AddOrOverwrite(typeof(UIImageView), nameof(UIImageView.Image));
            registry.AddOrOverwrite(typeof(UIDatePicker), MvxIosPropertyBinding.UIDatePicker_Date);
            registry.AddOrOverwrite(typeof(UISlider), MvxIosPropertyBinding.UISlider_Value);
            registry.AddOrOverwrite(typeof(UISwitch), MvxIosPropertyBinding.UISwitch_On);
            registry.AddOrOverwrite(typeof(UIProgressView), nameof(UIProgressView.Progress));
            registry.AddOrOverwrite(typeof(IMvxImageHelper<UIImage>), nameof(IMvxImageHelper<UIImage>.ImageUrl));
            registry.AddOrOverwrite(typeof(MvxImageViewLoader), nameof(MvxImageViewLoader.ImageUrl));
            registry.AddOrOverwrite(typeof(UISegmentedControl), MvxIosPropertyBinding.UISegmentedControl_SelectedSegment);
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), MvxIosPropertyBinding.UIActivityIndicatorView_Hidden);

            this._fillBindingNamesAction?.Invoke(registry);
        }
    }
}
