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
            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUISliderValueTargetBinding), typeof (UISlider),
                                               "Value");
            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUIDatePickerDateTargetBinding),
                                               typeof (UIDatePicker),
                                               "Date");

            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUITextFieldTextTargetBinding), typeof (UITextField),
                                               "Text");
            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUITextViewTextTargetBinding), typeof (UITextView),
                                               "Text");

            registry.RegisterPropertyInfoBindingFactory(typeof (MvxUISwitchOnTargetBinding), typeof (UISwitch), "On");
            registry.RegisterCustomBindingFactory<UIButton>("Title",
                                                        (button) => new MvxUIButtonTitleTargetBinding(button));

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

            registry.AddOrOverwrite<UITextField>(t => t.Text);
            registry.AddOrOverwrite<UITextView>(t => t.Text);
            registry.AddOrOverwrite<UILabel>(t => t.Text);
            registry.AddOrOverwrite<UITextField>(t => t.Text);
            registry.AddOrOverwrite<MvxCollectionViewSource>(c => c.ItemsSource);
            registry.AddOrOverwrite<MvxTableViewSource>(c => c.ItemsSource);
            registry.AddOrOverwrite<MvxImageView>(t => t.ImageUrl);
            registry.AddOrOverwrite<UIImageView>(t => t.Image);
            registry.AddOrOverwrite<UIDatePicker>(t => t.Date);
            registry.AddOrOverwrite<UISlider>(t => t.Value);
            registry.AddOrOverwrite<UISwitch>(t => t.On);
            registry.AddOrOverwrite<UIDatePicker>(t => t.Date);
            registry.AddOrOverwrite<IMvxImageHelper<UIImage>>(t => t.ImageUrl);
            registry.AddOrOverwrite<MvxImageViewLoader>(t => t.ImageUrl);

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
        }
    }
}