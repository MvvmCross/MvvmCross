// MvxAndroidBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<MvxViewTypeResolver> _setupViewTypeResolver;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;

        public MvxAndroidBindingBuilder(
            Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
            Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
            Action<IMvxBindingNameRegistry> fillBindingNamesAction = null,
            Action<MvxViewTypeResolver> setupViewTypeResolver = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;
            _setupViewTypeResolver = setupViewTypeResolver;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            base.RegisterPropertyInfoBindingFactory(registry, typeof (MvxTextViewTextTargetBinding), typeof (TextView),
                                                    "Text");
            base.RegisterPropertyInfoBindingFactory(registry, (typeof (MvxAutoCompleteTextViewPartialTextTargetBinding)),
                                                    typeof (AutoCompleteTextView), "PartialText");
            base.RegisterPropertyInfoBindingFactory(registry,
                                                    typeof (MvxAutoCompleteTextViewSelectedObjectTargetBinding),
                                                    typeof (AutoCompleteTextView),
                                                    "SelectedObject");
            base.RegisterPropertyInfoBindingFactory(registry, typeof (MvxCompoundButtonCheckedTargetBinding),
                                                    typeof (CompoundButton), "Checked");
            base.RegisterPropertyInfoBindingFactory(registry, typeof (MvxSeekBarProgressTargetBinging), typeof (SeekBar),
                                                    "Progress");
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("AssetImagePath",
                                                                            imageView =>
                                                                            new MvxImageViewImageTargetBinding(
                                                                                imageView)));
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("DrawableId",
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
            registry.RegisterFactory(new MvxCustomBindingFactory<View>("LongClick",
                                                                       view =>
                                                                       new MvxViewLongClickBinding(view)));

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

            registry.AddOrOverwrite(typeof (Button), "TouchUpInside");
            registry.AddOrOverwrite(typeof (CheckBox), "Checked");
            registry.AddOrOverwrite(typeof (TextView), "Text");
            registry.AddOrOverwrite(typeof (MvxListView), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxLinearLayout), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxGridView), "ItemsSource");
            registry.AddOrOverwrite(typeof (MvxImageView), "ImageUrl");
            registry.AddOrOverwrite(typeof (MvxDatePicker), "Value");
            registry.AddOrOverwrite(typeof (MvxTimePicker), "Value");
            registry.AddOrOverwrite(typeof (CompoundButton), "Checked");
            registry.AddOrOverwrite(typeof (SeekBar), "Progress");
            registry.AddOrOverwrite(typeof (IMvxImageHelper<Bitmap>), "ImageUrl");

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
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
            Mvx.RegisterSingleton(stack);
        }

        protected virtual IMvxBindingContextStack<IMvxAndroidBindingContext> CreateContextStack()
        {
            return new MvxAndroidBindingContextStack();
        }

        protected virtual void InitialiseViewTypeResolver()
        {
            var viewTypeResolver = new MvxViewTypeResolver();
            Mvx.RegisterSingleton<IMvxViewTypeResolver>(viewTypeResolver);

            if (_setupViewTypeResolver != null)
                _setupViewTypeResolver(viewTypeResolver);
        }
    }
}