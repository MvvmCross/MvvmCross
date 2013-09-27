// MvxTouchBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.CrossCore.Converters;
using Cirrious.MvvmCross.Binding.BindingContext;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Binding.Mac.Target;

namespace Cirrious.MvvmCross.Binding.Mac
{
    public class MvxMacBindingBuilder
        : MvxBindingBuilder
    {
		private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
		private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
		private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;

		public MvxMacBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
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

			registry.RegisterCustomBindingFactory<NSView>("Visibility",
			                                              view =>
			                                              new MvxNSViewVisibilityTargetBinding(view));
			registry.RegisterCustomBindingFactory<NSView>("Visible",
			                                              view =>
			                                              new MvxNSViewVisibleTargetBinding(view));

			registry.RegisterCustomBindingFactory<NSCollectionView>("ItemsSource",
			                                              view =>
			                                              new MvxNSCollectionViewItemsSourceTargetBinding(view));

			registry.RegisterPropertyInfoBindingFactory(typeof(MvxNSSliderValueTargetBinding), typeof(NSSlider),
			                                            "FloatValue");
			registry.RegisterPropertyInfoBindingFactory(typeof (MvxNSDatePickerDateTargetBinding),
			                                            typeof (NSDatePicker),
			                                            "Date");
			registry.RegisterCustomBindingFactory<NSDatePicker>(
				"Time",
				view => new MvxNSDatePickerTimeTargetBinding(view, (typeof(NSDatePicker).GetProperty("Date"))));

			registry.RegisterPropertyInfoBindingFactory(typeof (MvxNSTextFieldTextTargetBinding), typeof (NSTextField),
			                                            "StringValue");
			registry.RegisterCustomBindingFactory<NSTextView>("Text",
			                                                  view => new MvxNSTextViewTextTargetBinding(view));

			registry.RegisterPropertyInfoBindingFactory(typeof (MvxNSSwitchOnTargetBinding), typeof (NSButton), "On");
			registry.RegisterPropertyInfoBindingFactory(typeof(MvxNSSearchFieldTextTargetBinding), typeof(NSSearchField), "Text");

			registry.RegisterCustomBindingFactory<NSButton>("Title",
			                                                (button) => new MvxNSButtonTitleTargetBinding(button));

			/* Todo: Address this for trackpad
			registry.RegisterCustomBindingFactory<NSView>("Tap", view => new MvxNSViewTapTargetBinding(view));
			registry.RegisterCustomBindingFactory<NSView>("DoubleTap", view => new MvxNSViewTapTargetBinding(view, 2, 1));
			registry.RegisterCustomBindingFactory<NSView>("TwoFingerTap", view => new MvxNSViewTapTargetBinding(view, 1, 2));
			*/

            if (_fillRegistryAction != null)
                _fillRegistryAction(registry);
        }

        protected virtual void RegisterPropertyInfoBindingFactory(IMvxTargetBindingFactoryRegistry registry,
                                                                  Type bindingType, Type targetType, string targetName)
        {
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
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

			registry.AddOrOverwrite(typeof (NSButton), "Activated");
			registry.AddOrOverwrite(typeof (NSSegmentedControl), "Clicked");

			registry.AddOrOverwrite(typeof (NSSearchField), "Text");
			registry.AddOrOverwrite(typeof (NSTextField), "StringValue");
			registry.AddOrOverwrite(typeof (NSTextView), "Text");
			registry.AddOrOverwrite(typeof (NSCollectionView), "ItemsSource");
//			registry.AddOrOverwrite(typeof (MvxCollectionView), "ItemsSource");
//			registry.AddOrOverwrite(typeof (MvxTableViewSource), "ItemsSource");
//			registry.AddOrOverwrite(typeof (MvxImageView), "ImageUrl");
			registry.AddOrOverwrite(typeof (NSImageView), "Image");
			registry.AddOrOverwrite(typeof (NSDatePicker), "Date");
			registry.AddOrOverwrite(typeof (NSSlider), "FloatValue");
			registry.AddOrOverwrite(typeof (NSButton), "On");
			registry.AddOrOverwrite(typeof (NSDatePicker), "Date");
//			registry.AddOrOverwrite(typeof (IMvxImageHelper<UIImage>), "ImageUrl");
//			registry.AddOrOverwrite(typeof (MvxImageViewLoader), "ImageUrl");

			if (_fillBindingNamesAction != null)
				_fillBindingNamesAction(registry);
		}
	}
}