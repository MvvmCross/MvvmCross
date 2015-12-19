// MvxTouchBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch
{
    using System;

    using MvvmCross.Binding.Binders;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Binding.Touch.Target;
    using MvvmCross.Binding.Touch.ValueConverters;
    using MvvmCross.Binding.Touch.Views;
    using MvvmCross.Platform.Converters;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class MvxTouchBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<IMvxAutoValueConverters> _fillAutoValueConvertersAction;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;
        private readonly MvxUnifiedTypesValueConverter _unifiedValueTypesConverter;

        public MvxTouchBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
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

            registry.RegisterCustomBindingFactory<UIControl>("TouchUpInside",
                                                             view =>
                                                             new MvxUIControlTouchUpInsideTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("Visibility",
                                                          view =>
                                                          new MvxUIViewVisibilityTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("Visible",
                                                          view =>
                                                          new MvxUIViewVisibleTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIActivityIndicatorView>("Hidden",
                                                                           activityIndicator => new MvxUIActivityIndicatorViewHiddenTargetBinding(activityIndicator));
            registry.RegisterCustomBindingFactory<UIView>("Hidden",
                                                          view =>
                                                          new MvxUIViewHiddenTargetBinding(view));
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISliderValueTargetBinding),
                                                        typeof(UISlider),
                                                        "Value");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISegmentedControlSelectedSegmentTargetBinding),
                                                        typeof(UISegmentedControl),
                                                        "SelectedSegment");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUIDatePickerDateTargetBinding),
                                                        typeof(UIDatePicker),
                                                        "Date");
            registry.RegisterCustomBindingFactory<UITextField>("ShouldReturn",
                                                               textField => new MvxUITextFieldShouldReturnTargetBinding(textField));
            registry.RegisterCustomBindingFactory<UIDatePicker>("Time",
                                                                view => new MvxUIDatePickerTimeTargetBinding(view, (typeof(UIDatePicker).GetProperty("Date"))));

            registry.RegisterCustomBindingFactory<UILabel>("Text",
                                                           view => new MvxUILabelTextTargetBinding(view));
            registry.RegisterCustomBindingFactory<UITextField>("Text",
                                                               view => new MvxUITextFieldTextTargetBinding(view));
            registry.RegisterCustomBindingFactory<UITextView>("Text",
                                                              view => new MvxUITextViewTextTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("LayerBorderWidth",
                                                          view => new MvxUIViewLayerBorderWidthTargetBinding(view));
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISwitchOnTargetBinding),
                                                        typeof(UISwitch),
                                                        "On");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISearchBarTextTargetBinding),
                                                        typeof(UISearchBar),
                                                        "Text");

            registry.RegisterCustomBindingFactory<UIButton>("Title",
                                                            button => new MvxUIButtonTitleTargetBinding(button));
            registry.RegisterCustomBindingFactory<UIButton>("DisabledTitle",
                                                            button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Disabled));
            registry.RegisterCustomBindingFactory<UIButton>("HighlightedTitle",
                                                            button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Highlighted));
            registry.RegisterCustomBindingFactory<UIButton>("SelectedTitle",
                                                            button => new MvxUIButtonTitleTargetBinding(button, UIControlState.Selected));
            registry.RegisterCustomBindingFactory<UIView>("Tap",
                                                          view => new MvxUIViewTapTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("DoubleTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 2, 1));
            registry.RegisterCustomBindingFactory<UIView>("TwoFingerTap",
                                                          view => new MvxUIViewTapTargetBinding(view, 1, 2));
            registry.RegisterCustomBindingFactory<UITextField>("TextFocus",
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

            registry.AddOrOverwrite(typeof(UIButton), "TouchUpInside");
            registry.AddOrOverwrite(typeof(UIBarButtonItem), "Clicked");

            registry.AddOrOverwrite(typeof(UISearchBar), "Text");
            registry.AddOrOverwrite(typeof(UITextField), "Text");
            registry.AddOrOverwrite(typeof(UITextView), "Text");
            registry.AddOrOverwrite(typeof(UILabel), "Text");
            registry.AddOrOverwrite(typeof(MvxCollectionViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxTableViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxImageView), "ImageUrl");
            registry.AddOrOverwrite(typeof(UIImageView), "Image");
            registry.AddOrOverwrite(typeof(UIDatePicker), "Date");
            registry.AddOrOverwrite(typeof(UISlider), "Value");
            registry.AddOrOverwrite(typeof(UISwitch), "On");
            registry.AddOrOverwrite(typeof(UIProgressView), "Progress");
            registry.AddOrOverwrite(typeof(IMvxImageHelper<UIImage>), "ImageUrl");
            registry.AddOrOverwrite(typeof(MvxImageViewLoader), "ImageUrl");
            registry.AddOrOverwrite(typeof(UISegmentedControl), "SelectedSegment");
            registry.AddOrOverwrite(typeof(UIActivityIndicatorView), "Hidden");

            this._fillBindingNamesAction?.Invoke(registry);
        }
    }
}