// MvxTouchBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch.Target;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;

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
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUISliderValueTargetBinding), typeof (UISlider),
                                               "Value");
            RegisterPropertyInfoBindingFactory(registry, typeof (MvxUITextFieldTextTargetBinding), typeof (UITextField),
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

		protected override void FillDefaultBindingNames (IMvxBindingNameRegistry registry)
		{
			base.FillDefaultBindingNames (registry);

#warning Much more to do here
			registry.AddOrOverwrite(typeof(UIButton), "TouchUpInside");
			registry.AddOrOverwrite(typeof(UITextField), "Text");
			registry.AddOrOverwrite(typeof(UILabel), "Text");

			if (_fillBindingNamesAction != null)
				_fillBindingNamesAction(registry);
		}
    }
}