// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Android.Webkit;
using AndroidX.Preference;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android.Binding.Binders;
using MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.ResourceHelpers;
using MvvmCross.Platforms.Android.Binding.Target;
using MvvmCross.Platforms.Android.Binding.Views;
using AppCompatSearchView = AndroidX.AppCompat.Widget.SearchView;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace MvvmCross.Platforms.Android.Binding
{
    public class MvxAndroidBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxValueConverterRegistry> _fillValueConverters;
        private readonly Action<IMvxValueCombinerRegistry> _fillValueCombiners;
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillTargetFactories;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNames;
        private readonly Action<IMvxViewTypeRegistry> _fillViewTypes;
        private readonly Action<IMvxAxmlNameViewTypeResolver> _fillAxmlViewTypeResolver;
        private readonly Action<IMvxNamespaceListViewTypeResolver> _fillNamespaceListViewTypeResolver;

        public MvxAndroidBindingBuilder(
            Action<IMvxValueConverterRegistry> fillValueConverters,
            Action<IMvxValueCombinerRegistry> fillValueCombiners,
            Action<IMvxTargetBindingFactoryRegistry> fillTargetFactories,
            Action<IMvxBindingNameRegistry> fillBindingNames,
            Action<IMvxViewTypeRegistry> fillViewTypes,
            Action<IMvxAxmlNameViewTypeResolver> fillAxmlViewTypeResolver,
            Action<IMvxNamespaceListViewTypeResolver> fillNamespaceListViewTypeResolver)
        {
            _fillValueConverters = fillValueConverters;
            _fillValueCombiners = fillValueCombiners;
            _fillTargetFactories = fillTargetFactories;
            _fillBindingNames = fillBindingNames;
            _fillViewTypes = fillViewTypes;
            _fillAxmlViewTypeResolver = fillAxmlViewTypeResolver;
            _fillNamespaceListViewTypeResolver = fillNamespaceListViewTypeResolver;
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);
            _fillValueConverters?.Invoke(registry);
        }

        protected override void FillValueCombiners(IMvxValueCombinerRegistry registry)
        {
            base.FillValueCombiners(registry);
            _fillValueCombiners?.Invoke(registry);
        }

        [RequiresUnreferencedCode("This method registers source steps that may not be preserved by trimming")]
        public override void DoRegistration(IServiceCollection services)
        {
            InitializeAppResourceTypeFinder(services);
            InitializeBindingResources(services);
            InitializeLayoutInflation(services);
            base.DoRegistration(services);
        }

        protected virtual void InitializeLayoutInflation(IServiceCollection services)
        {
            var inflaterfactoryFactory = CreateLayoutInflaterFactoryFactory();
            services.TryAddSingleton<IMvxLayoutInflaterHolderFactoryFactory>(_ => inflaterfactoryFactory);

            var viewFactory = CreateAndroidViewFactory();
            services.TryAddSingleton<IMvxAndroidViewFactory>(_ => viewFactory);

            var viewBinderFactory = CreateAndroidViewBinderFactory();
            services.TryAddSingleton<IMvxAndroidViewBinderFactory>(_ => viewBinderFactory);
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

        protected virtual void InitializeBindingResources(IServiceCollection services)
        {
            var mvxAndroidBindingResource = CreateAndroidBindingResource();
            services.TryAddSingleton<IMvxAndroidBindingResource>(_ => mvxAndroidBindingResource);
        }

        protected virtual IMvxAndroidBindingResource CreateAndroidBindingResource()
        {
            return new MvxAndroidBindingResource();
        }

        protected virtual void InitializeAppResourceTypeFinder(IServiceCollection services)
        {
            var resourceFinder = CreateAppResourceTypeFinder();
            services.TryAddSingleton<IMvxAppResourceTypeFinder>(_ => resourceFinder);
        }

        protected virtual IMvxAppResourceTypeFinder CreateAppResourceTypeFinder()
        {
            return new MvxAppResourceTypeFinder();
        }

        [RequiresUnreferencedCode("This method registers target bindings that may not be preserved by trimming")]
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

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAutoCompleteTextViewPartialTextTargetBinding),
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

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxNumberPickerValueTargetBinding),
                typeof(NumberPicker),
                MvxAndroidPropertyBinding.NumberPicker_Value);

            registry.RegisterCustomBindingFactory<NumberPicker>(
                MvxAndroidPropertyBinding.NumberPicker_DisplayedValues,
                view => new MvxNumberPickerDisplayedValuesTargetBinding(view));

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

            registry.RegisterCustomBindingFactory<EditText>(
                MvxAndroidPropertyBinding.EditText_TextFocus,
                editText => new MvxTextViewFocusTargetBinding(editText));

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

            var allMargins = new[]
            {
                MvxAndroidPropertyBinding.View_Margin,
                MvxAndroidPropertyBinding.View_MarginLeft,
                MvxAndroidPropertyBinding.View_MarginRight,
                MvxAndroidPropertyBinding.View_MarginTop,
                MvxAndroidPropertyBinding.View_MarginBottom,
                MvxAndroidPropertyBinding.View_MarginStart,
                MvxAndroidPropertyBinding.View_MarginEnd
            };

            foreach (var margin in allMargins)
            {
                registry.RegisterCustomBindingFactory<View>(
                    margin, view => new MvxViewMarginTargetBinding(view, margin));
            }

            registry.RegisterCustomBindingFactory<View>(
                MvxAndroidPropertyBinding.View_Focus,
                view => new MvxViewFocusChangedTargetBinding(view));

            registry.RegisterCustomBindingFactory<VideoView>(
                MvxAndroidPropertyBinding.VideoView_Uri,
                view => new MvxVideoViewUriTargetBinding(view));

            registry.RegisterCustomBindingFactory<WebView>(
                MvxAndroidPropertyBinding.WebView_Uri,
                view => new MvxWebViewUriTargetBinding(view));

            registry.RegisterCustomBindingFactory<WebView>(
                MvxAndroidPropertyBinding.WebView_Html,
                view => new MvxWebViewHtmlTargetBinding(view));

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding),
                typeof(MvxAppCompatAutoCompleteTextView),
                MvxAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_PartialText);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(MvxAppCompatAutoCompleteTextView),
                MvxAndroidPropertyBinding.MvxAppCompatAutoCompleteTextView_SelectedObject);

            registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>(
                MvxAndroidPropertyBinding.MvxAppCompatSpinner_SelectedItem,
                spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                MvxAndroidPropertyBinding.MvxAppCompatRadioGroup_SelectedItem,
                radioGroup => new MvxAppCompatRadioGroupSelectedItemBinding(radioGroup));

            registry.RegisterCustomBindingFactory<Toolbar>(
                MvxAndroidPropertyBinding.Toolbar_Subtitle,
                toolbar => new MvxToolbarSubtitleBinding(toolbar));

            registry.RegisterCustomBindingFactory<AppCompatSearchView>(
                MvxAndroidPropertyBinding.SearchView_Query,
                searchView => new MvxAppCompatSearchViewQueryTextTargetBinding(searchView));

            _fillTargetFactories?.Invoke(registry);
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
            registry.AddOrOverwrite(typeof(MvxFrameControl), nameof(MvxFrameControl.DataContext));
            registry.AddOrOverwrite(typeof(MvxDatePicker), nameof(MvxDatePicker.Value));
            registry.AddOrOverwrite(typeof(MvxTimePicker), nameof(MvxTimePicker.Value));
            registry.AddOrOverwrite(typeof(CompoundButton), MvxAndroidPropertyBinding.CompoundButton_Checked);
            registry.AddOrOverwrite(typeof(SeekBar), MvxAndroidPropertyBinding.SeekBar_Progress);
            registry.AddOrOverwrite(typeof(SearchView), MvxAndroidPropertyBinding.SearchView_Query);
            registry.AddOrOverwrite(typeof(AppCompatSearchView), MvxAndroidPropertyBinding.SearchView_Query);
            registry.AddOrOverwrite(typeof(NumberPicker), MvxAndroidPropertyBinding.NumberPicker_Value);
            registry.AddOrOverwrite(typeof(NumberPicker), MvxAndroidPropertyBinding.NumberPicker_DisplayedValues);
            registry.AddOrOverwrite(typeof(VideoView), MvxAndroidPropertyBinding.VideoView_Uri);
            registry.AddOrOverwrite(typeof(WebView), MvxAndroidPropertyBinding.WebView_Uri);

            _fillBindingNames?.Invoke(registry);
        }

        protected override void RegisterPlatformSpecificComponents(IServiceCollection services)
        {
            base.RegisterPlatformSpecificComponents(services);

            InitializeViewTypeResolver(services);
            InitializeContextStack(services);
        }

        protected virtual void InitializeContextStack(IServiceCollection services)
        {
            var stack = CreateContextStack();
            services.TryAddSingleton<IMvxBindingContextStack<IMvxAndroidBindingContext>>(_ => stack);
        }

        protected virtual IMvxBindingContextStack<IMvxAndroidBindingContext> CreateContextStack()
        {
            return new MvxAndroidBindingContextStack();
        }

        protected virtual void InitializeViewTypeResolver(IServiceCollection services)
        {
            // Registry is built lazily when first resolved; all IMvxViewTypeRegistration
            // descriptors (added via services.AddMvvmCrossAndroidViewType<TView>()) are applied at that point.
            services.TryAddSingleton<IMvxViewTypeRegistry>(sp =>
            {
                var registry = new MvxViewTypeRegistry();
                foreach (var reg in sp.GetServices<IMvxViewTypeRegistration>())
                    reg.Apply(registry);
                _fillViewTypes?.Invoke(registry);
                return registry;
            });

            services.TryAddSingleton<IMvxAxmlNameViewTypeResolver>(sp =>
            {
                var registry = sp.GetRequiredService<IMvxViewTypeRegistry>();
                var resolver = new MvxAxmlNameViewTypeResolver(registry);
                _fillAxmlViewTypeResolver?.Invoke(resolver);
                return resolver;
            });

            services.TryAddSingleton<IMvxNamespaceListViewTypeResolver>(sp =>
            {
                var registry = sp.GetRequiredService<IMvxViewTypeRegistry>();
                var resolver = new MvxNamespaceListViewTypeResolver(registry);
                _fillNamespaceListViewTypeResolver?.Invoke(resolver);
                return resolver;
            });

            services.TryAddSingleton<IMvxViewTypeResolver>(sp =>
            {
                var fullNameResolver = sp.GetRequiredService<IMvxAxmlNameViewTypeResolver>();
                var listResolver = sp.GetRequiredService<IMvxNamespaceListViewTypeResolver>();
                var registry = sp.GetRequiredService<IMvxViewTypeRegistry>();
                var justNameResolver = new MvxJustNameViewTypeResolver(registry);
                var composite = new MvxCompositeViewTypeResolver(fullNameResolver, listResolver, justNameResolver);
                return new MvxCachedViewTypeResolver(composite);
            });
        }
    }
}
