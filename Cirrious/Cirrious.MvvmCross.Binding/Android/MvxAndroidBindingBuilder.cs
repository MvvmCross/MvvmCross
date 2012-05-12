#region Copyright
// <copyright file="MvxAndroidBindingBuilder.cs" company="Cirrious">
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
using Android.Widget;
using Cirrious.MvvmCross.Binding.Android.Binders;
using Cirrious.MvvmCross.Binding.Android.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Android.Target;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Android
{
    public class MvxAndroidBindingBuilder
        : MvxBaseBindingBuilder
        , IMvxServiceProducer<IMvxViewTypeResolver>
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<MvxViewTypeResolver> _setupViewTypeResolver;

        public MvxAndroidBindingBuilder(
            Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction, 
            Action<IMvxValueConverterRegistry> fillValueConvertersAction,
            Action<MvxViewTypeResolver> setupViewTypeResolver)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _setupViewTypeResolver = setupViewTypeResolver;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxEditTextTextTargetBinding), typeof(EditText), "Text"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxAutoCompleteTextViewPartialTextTargetBinding), typeof(AutoCompleteTextView), "PartialText"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxAutoCompleteTextViewSelectedObjectTargetBinding), typeof(AutoCompleteTextView), "SelectedObject"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxCompoundButtonCheckedTargetBinding), typeof(CompoundButton), "Checked"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxSeekBarProgressTargetBinging), typeof(SeekBar), "Progress"));
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("AssetImagePath", (imageView) => new MvxImageViewDrawableTargetBinding(imageView)));

            if (_fillRegistryAction != null)
                _fillRegistryAction(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (_fillValueConvertersAction != null)
                _fillValueConvertersAction(registry);
        }

        protected override void RegisterPlatformSpecificComponents()
        {
            base.RegisterPlatformSpecificComponents();

            InitialiseViewTypeResolver();
        }

        private void InitialiseViewTypeResolver()
        {
            var viewTypeResolver = new MvxViewTypeResolver();
            _setupViewTypeResolver(viewTypeResolver);
            this.RegisterServiceInstance<IMvxViewTypeResolver>(viewTypeResolver);
        }
    }
}