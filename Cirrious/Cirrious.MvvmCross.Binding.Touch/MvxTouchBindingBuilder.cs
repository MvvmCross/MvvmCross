// MvxTouchBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch.Target;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch
{
    public class MvxTouchBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;

        public MvxTouchBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
                                      Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
                                      Action<IMvxBindingNameRegistry> fillBindingNamesAction = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<UIView>("Visibility",
                                                        view =>
                                                        new MvxUIViewVisibilityTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("Visible",
                                                        view =>
                                                        new MvxUIViewVisibleTargetBinding(view));
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISliderValueTargetBinding), typeof(UISlider),
                                               "Value");
            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUIDatePickerDateTargetBinding),
                                               typeof (UIDatePicker),
                                               "Date");
            registry.RegisterCustomBindingFactory<UIDatePicker>(
                                               "Time",
                                               view => new MvxUIDatePickerTimeTargetBinding(view, (typeof(UIDatePicker).GetProperty("Date"))));

            registry.RegisterCustomBindingFactory<UITextField>(
                                               "Text",
                                               view => new MvxUITextFieldTextTargetBinding(view));
            registry.RegisterCustomBindingFactory<UITextView>(
                                               "Text",
                                               view => new MvxUITextViewTextTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUISwitchOnTargetBinding), typeof (UISwitch), "On");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISearchBarTextTargetBinding), typeof(UISearchBar), "Text");

            registry.RegisterCustomBindingFactory<UIButton>("Title",
                                                        (button) => new MvxUIButtonTitleTargetBinding(button));
            registry.RegisterCustomBindingFactory<UIView>("Tap", view => new MvxUIViewTapTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("DoubleTap", view => new MvxUIViewTapTargetBinding(view, 2, 1));
            registry.RegisterCustomBindingFactory<UIView>("TwoFingerTap", view => new MvxUIViewTapTargetBinding(view, 1, 2));
            /*
            registry.RegisterCustomBindingFactory<UIView>("TwoFingerDoubleTap", view => new MvxUIViewTapTargetBinding(view, 2, 2));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerTap", view => new MvxUIViewTapTargetBinding(view, 1, 3));
            registry.RegisterCustomBindingFactory<UIView>("ThreeFingerDoubleTap", view => new MvxUIViewTapTargetBinding(view, 3, 3));
            */

            if (_fillRegistryAction != null)
                _fillRegistryAction(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (_fillValueConvertersAction != null)
                _fillValueConvertersAction(registry);
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof (UIButton), "TouchUpInside");
            registry.AddOrOverwrite(typeof (UIBarButtonItem), "Clicked");

            registry.AddOrOverwrite(typeof (UISearchBar), "Text");
            registry.AddOrOverwrite(typeof (UITextField), "Text");
            registry.AddOrOverwrite(typeof (UITextView), "Text");
            registry.AddOrOverwrite(typeof (UILabel), "Text");
            registry.AddOrOverwrite(typeof (UITextField), "Text");
            registry.AddOrOverwrite(typeof (MvxCollectionViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxTableViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxImageView), "ImageUrl");
            registry.AddOrOverwrite(typeof (UIImageView), "Image");
            registry.AddOrOverwrite(typeof (UIDatePicker), "Date");
            registry.AddOrOverwrite(typeof (UISlider), "Value");
            registry.AddOrOverwrite(typeof (UISwitch), "On");
            registry.AddOrOverwrite(typeof (UIDatePicker), "Date");
            registry.AddOrOverwrite(typeof (IMvxImageHelper<UIImage>), "ImageUrl");
            registry.AddOrOverwrite(typeof (MvxImageViewLoader), "ImageUrl");

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
        }
    }
}