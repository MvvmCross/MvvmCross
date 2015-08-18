// MvxAndroidBindingBuilder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Binders;
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
            InitializeAppResourceTypeFinder();
            InitializeBindingResources();
            InitializeLayoutInflation();
            base.DoRegistration();
        }

        protected virtual void InitializeLayoutInflation()
        {
            var inflaterfactoryFactory = CreateLayoutInflaterFactoryFactory();
            Mvx.RegisterSingleton(inflaterfactoryFactory);

            var viewFactory = CreateAndroidViewFactory();
            Mvx.RegisterSingleton(viewFactory);

            var viewBinderFactory = CreateAndroidViewBinderFactory();
            Mvx.RegisterSingleton(viewBinderFactory);
        }

        protected virtual IMvxAndroidViewBinderFactory CreateAndroidViewBinderFactory()
        {
            return new MvxAndroidViewBinderFactory(); 
        }

        protected virtual IMvxLayoutInflaterHolderFactoryFactory CreateLayoutInflaterFactoryFactory()
        {
            return new MvxLayoutInflaterFactoryFactory();
        }

        protected virtual IMvxAndroidViewFactory CreateAndroidViewFactory()
        {
            return new MvxAndroidViewFactory();
        }

        protected virtual void InitializeBindingResources()
        {
            MvxAndroidBindingResource.Initialize();
        }

        protected virtual void InitializeAppResourceTypeFinder()
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

            registry.RegisterCustomBindingFactory<View>("Click",
                                                            view => new MvxViewClickBinding(view));
            registry.RegisterCustomBindingFactory<TextView>("Text",
                                                            textView => new MvxTextViewTextTargetBinding(textView));
            registry.RegisterCustomBindingFactory<TextView>("TextFormatted",
                                                            textView => new MvxTextViewTextFormattedTargetBinding(textView));
            registry.RegisterPropertyInfoBindingFactory((typeof(MvxAutoCompleteTextViewPartialTextTargetBinding)),
                                                    typeof(AutoCompleteTextView), "PartialText");
            registry.RegisterPropertyInfoBindingFactory(
                                                    typeof(MvxAutoCompleteTextViewSelectedObjectTargetBinding),
                                                    typeof(AutoCompleteTextView),
                                                    "SelectedObject");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxCompoundButtonCheckedTargetBinding),
                                                    typeof(CompoundButton), "Checked");
			registry.RegisterPropertyInfoBindingFactory(typeof(MvxSeekBarProgressTargetBinding), typeof(SeekBar),
                                                    "Progress");
            registry.RegisterCustomBindingFactory<View>("Visible",
                                                            view => new MvxViewVisibleBinding(view));
            registry.RegisterCustomBindingFactory<View>("Hidden",
                                                            view => new MvxViewHiddenBinding(view));
            registry.RegisterCustomBindingFactory<ImageView>("Bitmap",
                                                            imageView => new MvxImageViewBitmapTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<ImageView>("DrawableId",
                                                            imageView => new MvxImageViewDrawableTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<ImageView>("DrawableName",
                                                            imageView => new MvxImageViewDrawableNameTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<ImageView>("AssetImagePath",
                                                             imageView => new MvxImageViewImageTargetBinding(imageView)); 
            registry.RegisterCustomBindingFactory<MvxSpinner>("SelectedItem",
                                                                             spinner =>
                                                                             new MvxSpinnerSelectedItemBinding(
                                                                                 spinner));
            registry.RegisterCustomBindingFactory<AdapterView>("SelectedItemPosition",
                                                                              adapterView =>
                                                                              new MvxAdapterViewSelectedItemPositionTargetBinding
                                                                                  (adapterView));
            registry.RegisterCustomBindingFactory<MvxListView>("SelectedItem",
                                                                              adapterView =>
                                                                              new MvxListViewSelectedItemTargetBinding
                                                                                  (adapterView));
            registry.RegisterCustomBindingFactory<MvxExpandableListView>("SelectedItem",
                                                                         adapterView =>
                                                                             new MvxExpandableListViewSelectedItemTargetBinding(adapterView));
            registry.RegisterCustomBindingFactory<RatingBar>("Rating",
                                                            ratingBar => new MvxRatingBarRatingTargetBinding(ratingBar));
            registry.RegisterCustomBindingFactory<View>("LongClick",
                                                            view =>
                                                            new MvxViewLongClickBinding(view));
            registry.RegisterCustomBindingFactory<MvxRadioGroup>("SelectedItem",
                radioGroup => new MvxRadioGroupSelectedItemBinding(radioGroup));
			registry.RegisterCustomBindingFactory("TextFocus", (EditText view) => new MvxTextViewFocusTargetBinding(view));
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(Button), "Click");
            registry.AddOrOverwrite(typeof(CheckBox), "Checked");

            registry.AddOrOverwrite(typeof(TextView), "Text");
            registry.AddOrOverwrite(typeof(MvxListView), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxLinearLayout), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxGridView), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxRelativeLayout), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxFrameLayout), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxTableLayout), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxFrameControl), "DataContext");
            registry.AddOrOverwrite(typeof(MvxImageView), "ImageUrl");
            registry.AddOrOverwrite(typeof(MvxDatePicker), "Value");
            registry.AddOrOverwrite(typeof(MvxTimePicker), "Value");
            registry.AddOrOverwrite(typeof(CompoundButton), "Checked");
            registry.AddOrOverwrite(typeof(SeekBar), "Progress");
            registry.AddOrOverwrite(typeof(IMvxImageHelper<Bitmap>), "ImageUrl");
        }

        protected override void RegisterPlatformSpecificComponents()
        {
            base.RegisterPlatformSpecificComponents();

            InitializeViewTypeResolver();
            InitializeContextStack();
        }

        protected virtual void InitializeContextStack()
        {
            var stack = CreateContextStack();
            Mvx.RegisterSingleton(stack);
        }

        protected virtual IMvxBindingContextStack<IMvxAndroidBindingContext> CreateContextStack()
        {
            return new MvxAndroidBindingContextStack();
        }

        protected virtual void InitializeViewTypeResolver()
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