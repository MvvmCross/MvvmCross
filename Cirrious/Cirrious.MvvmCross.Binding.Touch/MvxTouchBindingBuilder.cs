// MvxTouchBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
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

            registry.RegisterFactory(new MvxCustomBindingFactory<UIView>("Visibility",
                                                                         view =>
                                                                         new MvxUIViewVisibilityTargetBinding(view)));
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUISliderValueTargetBinding), typeof (UISlider),
                                               "Value");
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUIDatePickerDateTargetBinding),
                                               typeof (UIDatePicker),
                                               "Date");

            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUISliderValueTargetBinding), typeof (UISlider),
                                               "Value");
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUITextFieldTextTargetBinding), typeof (UITextField),
                                               "Text");
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUITextViewTextTargetBinding), typeof (UITextView),
                                               "Text");

            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUISwitchOnTargetBinding), typeof (UISwitch), "On");
            registry.RegisterFactory(new MvxCustomBindingFactory<UIButton>("Title",
                                                                           (button) =>
                                                                           new MvxUIButtonTitleTargetBinding(button)));

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
            registry.AddOrOverwrite(typeof (UITextField), "Text");
            registry.AddOrOverwrite(typeof (UITextView), "Text");
            registry.AddOrOverwrite(typeof (UILabel), "Text");
            registry.AddOrOverwrite(typeof (MvxCollectionViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxTableViewSource), "ItemsSource");
			registry.AddOrOverwrite(typeof (MvxImageView), "ImageUrl");
			registry.AddOrOverwrite(typeof (UIImageView), "Image");
			registry.AddOrOverwrite(typeof (UIDatePicker), "Date");
            registry.AddOrOverwrite(typeof (UISlider), "Value");
            registry.AddOrOverwrite(typeof (UISwitch), "On");
            registry.AddOrOverwrite(typeof (IMvxImageHelper<UIImage>), "ImageUrl");
            registry.AddOrOverwrite(typeof (MvxImageViewLoader), "ImageUrl");

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
        }
    }
}