// MvxAndroidBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Widget;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingBuilder
        : MvxBindingBuilder
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

            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(
                                         typeof (MvxEditTextTextTargetBinding), typeof (EditText), "Text"));
            registry.RegisterFactory(
                new MvxSimplePropertyInfoTargetBindingFactory(typeof (MvxAutoCompleteTextViewPartialTextTargetBinding),
                                                              typeof (AutoCompleteTextView), "PartialText"));
            registry.RegisterFactory(
                new MvxSimplePropertyInfoTargetBindingFactory(
                    typeof (MvxAutoCompleteTextViewSelectedObjectTargetBinding), typeof (AutoCompleteTextView),
                    "SelectedObject"));
            registry.RegisterFactory(
                new MvxSimplePropertyInfoTargetBindingFactory(typeof (MvxCompoundButtonCheckedTargetBinding),
                                                              typeof (CompoundButton), "Checked"));
            registry.RegisterFactory(
                new MvxSimplePropertyInfoTargetBindingFactory(typeof (MvxSeekBarProgressTargetBinging), typeof (SeekBar),
                                                              "Progress"));
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("AssetImagePath",
                                                                            imageView =>
                                                                            new MvxImageViewDrawableTargetBinding(
                                                                                imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxSpinner>("SelectedItem",
                                                                             spinner =>
                                                                             new MvxSpinnerSelectedItemBinding(
                                                                                 spinner)));
            registry.RegisterFactory(new MvxCustomBindingFactory<AdapterView>("SelectedItemPosition",
                                                                              adapterView =>
                                                                              new MvxAdapterViewSelectedItemPositionTargetBinding
                                                                                  (adapterView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxListView>("SelectedItem",
                                                                              adapterView =>
                                                                              new MvxListViewSelectedItemTargetBinding
                                                                                  (adapterView)));

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
            InitialiseContextStack();
        }

        protected virtual void InitialiseContextStack()
        {
            var stack = CreateContextStack();
            this.RegisterServiceInstance(stack);
        }

        protected virtual IMvxBindingContextStack CreateContextStack()
        {
            return new MvxDroidBindingContextStack();
        }

        protected virtual void InitialiseViewTypeResolver()
        {
            var viewTypeResolver = new MvxViewTypeResolver();
            _setupViewTypeResolver(viewTypeResolver);
            this.RegisterServiceInstance<IMvxViewTypeResolver>(viewTypeResolver);
        }
    }
}