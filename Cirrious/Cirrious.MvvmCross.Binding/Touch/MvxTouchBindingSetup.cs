#region Copyright
// <copyright file="MvxTouchBindingSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch.Target;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch
{
    public abstract class MvxBaseTouchBindingSetup
    : MvxBaseTouchSetup
    {
        protected MvxBaseTouchBindingSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override void InitializeLastChance()
        {
            var bindingBuilder = new MvxTouchBindingBuilder(FillTargetFactories, FillValueConverters);
            bindingBuilder.DoRegistration();

            base.InitializeLastChance();
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            var holders = ValueConverterHolders;
            if (holders == null)
                return;

            var filler = new MvxInstanceBasedValueConverterRegistryFiller(registry);
            var staticFiller = new MvxStaticBasedValueConverterRegistryFiller(registry);
            foreach (var converterHolder in holders)
            {
                filler.AddFieldConverters(converterHolder);
                staticFiller.AddStaticFieldConverters(converterHolder);
            }
        }

        protected virtual IEnumerable<Type> ValueConverterHolders
        {
            get { return null; }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }

    public class MvxTouchBindingBuilder 
        : MvxBaseBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;

        public MvxTouchBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction, Action<IMvxValueConverterRegistry> fillValueConvertersAction)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            RegisterPropertyInfoBindingFactory(registry, typeof(MvxUISliderValueTargetBinding), typeof(UISlider), "Value");
            RegisterPropertyInfoBindingFactory(registry, typeof(MvxUITextFieldTextTargetBinding), typeof(UITextField), "Text");
            RegisterPropertyInfoBindingFactory(registry, typeof(MvxUISwitchOnTargetBinding), typeof(UISwitch), "On");
            registry.RegisterFactory(new MvxCustomBindingFactory<UIButton>("Title", (button) => new MvxUIButtonTitleTargetBinding(button)));
			
            if (_fillRegistryAction != null)
                _fillRegistryAction(registry);
        }

        protected static void RegisterPropertyInfoBindingFactory(IMvxTargetBindingFactoryRegistry registry, Type bindingType, Type targetType, string targetName)
        {
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(bindingType, targetType, targetName));
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (_fillValueConvertersAction != null)
                _fillValueConvertersAction(registry);
        }
    }
}