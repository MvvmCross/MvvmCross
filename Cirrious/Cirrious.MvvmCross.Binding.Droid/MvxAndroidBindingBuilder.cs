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
using Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Cirrious.MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingBuilder
        : MvxBindingBuilder
    {
        public override void DoRegistration()
        {
            InitialiseAppResourceTypeFinder();
            InitialiseBindingResources();
            base.DoRegistration();
        }

        protected virtual void InitialiseBindingResources()
        {
            MvxAndroidBindingResource.Initialise();
        }

        protected virtual void InitialiseAppResourceTypeFinder()
        {
            var resourceFinder = CreateAppResourceTypeFinder();
            Mvx.RegisterSingleton(resourceFinder);
        }

        protected virtual IMvxAppResourceTypeFinder CreateAppResourceTypeFinder()
        {
            return new MvxAppResourceTypeFinder();
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
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
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
            Mvx.RegisterSingleton<IMvxTypeCache<View>>(typeCache);

            var fullNameViewTypeResolver = new MvxAxmlNameViewTypeResolver(typeCache);
            Mvx.RegisterSingleton<IMvxAxmlNameViewTypeResolver>(fullNameViewTypeResolver);
            var listViewTypeResolver = new MvxNamespaceListViewTypeResolver(typeCache);
            Mvx.RegisterSingleton<IMvxNamespaceListViewTypeResolver>(listViewTypeResolver);
            var justNameTypeResolver = new MvxJustNameViewTypeResolver(typeCache);

            var composite = new MvxCompositeViewTypeResolver(fullNameViewTypeResolver, listViewTypeResolver, justNameTypeResolver);
            var cached = new MvxCachedViewTypeResolver(composite);
            Mvx.RegisterSingleton<IMvxViewTypeResolver>(cached);
        }

        protected virtual IMvxTypeCache<View> CreateViewTypeCache()
        {
            return new MvxTypeCache<View>();
        }
    }
}