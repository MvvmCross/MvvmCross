// MvxAndroidBindingBuilder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Graphics;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Droid.Binders;
using MvvmCross.Binding.Droid.Binders.ViewTypeResolvers;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.ResourceHelpers;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Binding.Droid
{
    public class MvxAndroidBindingBuilder
        : MvxBindingBuilder
    {
        public override void DoRegistration()
        {
            this.InitializeAppResourceTypeFinder();
            this.InitializeBindingResources();
            this.InitializeLayoutInflation();
            base.DoRegistration();
        }

        protected virtual void InitializeLayoutInflation()
        {
            var inflaterfactoryFactory = this.CreateLayoutInflaterFactoryFactory();
            Mvx.RegisterSingleton(inflaterfactoryFactory);

            var viewFactory = this.CreateAndroidViewFactory();
            Mvx.RegisterSingleton(viewFactory);

            var viewBinderFactory = this.CreateAndroidViewBinderFactory();
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
            var resourceFinder = this.CreateAppResourceTypeFinder();
            Mvx.RegisterSingleton(resourceFinder);
        }

        protected virtual IMvxAppResourceTypeFinder CreateAppResourceTypeFinder()
        {
            return new MvxAppResourceTypeFinder();
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_Click,
                view => new MvxViewClickBinding(view));

            registry.RegisterCustomBindingFactory<TextView>(
                MvxAndroidPropertyBinding.TextView_Text,
                textView => new MvxTextViewTextTargetBinding(textView));

            registry.RegisterCustomBindingFactory<TextView>(
                MvxAndroidPropertyBinding.TextView_TextFormatted,
                textView => new MvxTextViewTextFormattedTargetBinding(textView));

            registry.RegisterCustomBindingFactory<TextView>(
                MvxAndroidPropertyBinding.TextView_Hint,
                textView => new MvxTextViewHintTargetBinding(textView));

            registry.RegisterPropertyInfoBindingFactory(
                (typeof(MvxAutoCompleteTextViewPartialTextTargetBinding)),
                typeof(MvxAutoCompleteTextView),
                MvxAndroidPropertyBinding.MvxAutoCompleteTextView_PartialText);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(MvxAutoCompleteTextView),
                MvxAndroidPropertyBinding.MvxAutoCompleteTextView_SelectedObject);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxCompoundButtonCheckedTargetBinding),
                typeof(CompoundButton),
                MvxAndroidPropertyBinding.CompoundButton_Checked);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxSeekBarProgressTargetBinding), 
                typeof(SeekBar),
                MvxAndroidPropertyBinding.SeekBar_Progress);

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_Alpha,
                view => new MvxViewAlphaBinding(view));

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_Visible,
                view => new MvxViewVisibleBinding(view));

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_Hidden,
                view => new MvxViewHiddenBinding(view));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_Bitmap,
                imageView => new MvxImageViewBitmapTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_Drawable,
                imageView => new MvxImageViewImageDrawableTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_DrawableId,
                imageView => new MvxImageViewDrawableTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_DrawableName,
                imageView => new MvxImageViewDrawableNameTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_ResourceName,
                imageView => new MvxImageViewResourceNameTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<ImageView>(
                MvxAndroidPropertyBinding.ImageView_AssetImagePath,
                imageView => new MvxImageViewImageTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<MvxSpinner>(
                MvxAndroidPropertyBinding.MvxSpinner_SelectedItem,
                spinner => new MvxSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<AdapterView>(
                MvxAndroidPropertyBinding.AdapterView_SelectedItemPosition,
                adapterView => new MvxAdapterViewSelectedItemPositionTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<MvxListView>(
                MvxAndroidPropertyBinding.MvxListView_SelectedItem,
                adapterView => new MvxListViewSelectedItemTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<MvxExpandableListView>(
                MvxAndroidPropertyBinding.MvxExpandableListView_SelectedItem,
                adapterView => new MvxExpandableListViewSelectedItemTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<RatingBar>(
                MvxAndroidPropertyBinding.RatingBar_Rating,
                ratingBar => new MvxRatingBarRatingTargetBinding(ratingBar));

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_LongClick,
                view => new MvxViewLongClickBinding(view));

            registry.RegisterCustomBindingFactory<MvxRadioGroup>(
                MvxAndroidPropertyBinding.MvxRadioGroup_SelectedItem,
                radioGroup => new MvxRadioGroupSelectedItemBinding(radioGroup));

            registry.RegisterCustomBindingFactory(
                MvxAndroidPropertyBinding.EditText_TextFocus, 
                (EditText view) => new MvxTextViewFocusTargetBinding(view));

            registry.RegisterCustomBindingFactory<SearchView>(
                MvxAndroidPropertyBinding.SearchView_Query,
                search => new MvxSearchViewQueryTextTargetBinding(search));

            registry.RegisterCustomBindingFactory<Preference>(
                MvxAndroidPropertyBinding.Preference_Value,
                preference => new MvxPreferenceValueTargetBinding(preference));

            registry.RegisterCustomBindingFactory<EditTextPreference>(
                MvxAndroidPropertyBinding.EditTextPreference_Text,
                preference => new MvxEditTextPreferenceTextTargetBinding(preference));

            registry.RegisterCustomBindingFactory<ListPreference>(
                MvxAndroidPropertyBinding.ListPreference_Value,
                preference => new MvxListPreferenceTargetBinding(preference));

            registry.RegisterCustomBindingFactory<TwoStatePreference>(
                MvxAndroidPropertyBinding.TwoStatePreference_Checked,
                preference => new MvxTwoStatePreferenceCheckedTargetBinding(preference));
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(Button), MvxAndroidPropertyBinding.View_Click);
            registry.AddOrOverwrite(typeof(CheckBox), MvxAndroidPropertyBinding.CompoundButton_Checked);
            registry.AddOrOverwrite(typeof(TextView), MvxAndroidPropertyBinding.TextView_Text);
            registry.AddOrOverwrite(typeof(MvxListView), nameof(MvxListView.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxLinearLayout), nameof(MvxLinearLayout.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxGridView), nameof(MvxGridView.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxRelativeLayout), nameof(MvxRelativeLayout.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxFrameLayout), nameof(MvxFrameLayout.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxTableLayout), nameof(MvxTableLayout.ItemsSource));
            registry.AddOrOverwrite(typeof(MvxFrameControl), nameof(MvxFrameControl.DataContext));
            registry.AddOrOverwrite(typeof(MvxImageView), nameof(MvxImageView.ImageUrl));
            registry.AddOrOverwrite(typeof(MvxDatePicker), nameof(MvxDatePicker.Value));
            registry.AddOrOverwrite(typeof(MvxTimePicker), nameof(MvxTimePicker.Value));
            registry.AddOrOverwrite(typeof(CompoundButton), MvxAndroidPropertyBinding.CompoundButton_Checked);
            registry.AddOrOverwrite(typeof(SeekBar), MvxAndroidPropertyBinding.SeekBar_Progress);
            registry.AddOrOverwrite(typeof(IMvxImageHelper<Bitmap>), nameof(IMvxImageHelper<Bitmap>.ImageUrl));
            registry.AddOrOverwrite(typeof(SearchView), MvxAndroidPropertyBinding.SearchView_Query);
        }

        protected override void RegisterPlatformSpecificComponents()
        {
            base.RegisterPlatformSpecificComponents();

            this.InitializeViewTypeResolver();
            this.InitializeContextStack();
        }

        protected virtual void InitializeContextStack()
        {
            var stack = this.CreateContextStack();
            Mvx.RegisterSingleton(stack);
        }

        protected virtual IMvxBindingContextStack<IMvxAndroidBindingContext> CreateContextStack()
        {
            return new MvxAndroidBindingContextStack();
        }

        protected virtual void InitializeViewTypeResolver()
        {
            var typeCache = this.CreateViewTypeCache();
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
