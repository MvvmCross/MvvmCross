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
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Binders;
using Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
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
        private readonly Action<MvxAxmlNameViewTypeResolver> _setupAxmlNameViewTypeResolver;
        private readonly Action<MvxNamespaceListViewTypeResolver> _setupNamespaceListViewTypeResolver;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;
        private readonly Action<IMvxTypeCache<View>> _fillViewTypesAction;

        public MvxAndroidBindingBuilder(
            Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
            Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
            Action<IMvxBindingNameRegistry> fillBindingNamesAction = null,
            Action<MvxAxmlNameViewTypeResolver> setupAxmlNameViewTypeResolver = null,
            Action<MvxNamespaceListViewTypeResolver> setupNamespaceListViewTypeResolver = null,
            Action<IMvxTypeCache<View>> fillViewTypesAction = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;
            _setupAxmlNameViewTypeResolver = setupAxmlNameViewTypeResolver;
            _setupNamespaceListViewTypeResolver = setupNamespaceListViewTypeResolver;
            _fillViewTypesAction = fillViewTypesAction;
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
            registry.RegisterFactory(new MvxCustomBindingFactory<ImageView>("Bitmap",
                                                                            imageView =>
                                                                            new MvxImageViewBitmapTargetBinding(
                                                                                imageView)));
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

            registry.AddOrOverwrite(typeof (Button), "Click");
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
            var typeCache = CreateViewTypeCache();
            if (_fillViewTypesAction != null)
                _fillViewTypesAction(typeCache);

            var fullNameViewTypeResolver = new MvxAxmlNameViewTypeResolver(typeCache);
            var listViewTypeResolver = new MvxNamespaceListViewTypeResolver(typeCache);
            var justNameTypeResolver = new MvxJustNameViewTypeResolver(typeCache);

            var composite = new MvxCompositeViewTypeResolver(fullNameViewTypeResolver, listViewTypeResolver, justNameTypeResolver);
            var cached = new MvxCachedViewTypeResolver(composite);
            Mvx.RegisterSingleton<IMvxViewTypeResolver>(cached);

            if (_setupAxmlNameViewTypeResolver != null)
                _setupAxmlNameViewTypeResolver(fullNameViewTypeResolver);
            if (_setupNamespaceListViewTypeResolver != null)
                _setupNamespaceListViewTypeResolver(listViewTypeResolver);            
        }

        protected virtual IMvxTypeCache<View> CreateViewTypeCache()
        {
            return new MvxTypeCache<View>();
        }
    }
}