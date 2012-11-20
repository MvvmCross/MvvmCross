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
using Android.Widget;

using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingBuilder
        : MvxBaseBindingBuilder
        , IMvxServiceProducer
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
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("AssetImagePath", imageView => new MvxImageViewDrawableTargetBinding(imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxBindableSpinner>("SelectedItem", spinner => new MvxSpinnerSelectedItemBinding(spinner)));
            registry.RegisterFactory(new MvxCustomBindingFactory<AdapterView>("SelectedItemPosition", adapterView => new MvxAdapterViewSelectedItemPositionTargetBinding(adapterView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxBindableListView>("SelectedItem", adapterView => new MvxAdapterViewSelectedItemTargetBinding(adapterView)));

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