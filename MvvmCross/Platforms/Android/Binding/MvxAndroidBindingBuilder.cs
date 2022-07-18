// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.Preference;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.IoC;
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
        public override void DoRegistration(IMvxIoCProvider iocProvider)
        {
            InitializeAppResourceTypeFinder();
            InitializeBindingResources(iocProvider);
            InitializeLayoutInflation(iocProvider);
            base.DoRegistration(iocProvider);
        }

        protected virtual void InitializeLayoutInflation(IMvxIoCProvider iocProvider)
        {
            var inflaterfactoryFactory = CreateLayoutInflaterFactoryFactory();
            iocProvider.RegisterSingleton(inflaterfactoryFactory);

            var viewFactory = CreateAndroidViewFactory();
            iocProvider.RegisterSingleton(viewFactory);

            var viewBinderFactory = CreateAndroidViewBinderFactory();
            iocProvider.RegisterSingleton(viewBinderFactory);
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

        protected virtual void InitializeBindingResources(IMvxIoCProvider iocProvider)
        {
            var mvxAndroidBindingResource = CreateAndroidBindingResource();
            iocProvider.RegisterSingleton(mvxAndroidBindingResource);
        }

        protected virtual IMvxAndroidBindingResource CreateAndroidBindingResource()
        {
            return new MvxAndroidBindingResource();
        }

        protected virtual void InitializeAppResourceTypeFinder()
        {
            var resourceFinder = CreateAppResourceTypeFinder();
            Mvx.IoCProvider.RegisterSingleton(resourceFinder);
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
                view => new MvxViewFocusChangedTargetbinding(view));

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

            registry.RegisterCustomBindingFactory<MvxAppCompatSearchViewQueryTextTargetBinding>(
                MvxAndroidPropertyBinding.SearchView_Query,
                searchView => new MvxAppCompatSearchViewQueryTextTargetBinding(searchView));
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
        }

        protected override void RegisterPlatformSpecificComponents(IMvxIoCProvider iocProvider)
        {
            base.RegisterPlatformSpecificComponents(iocProvider);

            InitializeViewTypeResolver(iocProvider);
            InitializeContextStack(iocProvider);
        }

        protected virtual void InitializeContextStack(IMvxIoCProvider iocProvider)
        {
            var stack = CreateContextStack();
            iocProvider.RegisterSingleton(stack);
        }

        protected virtual IMvxBindingContextStack<IMvxAndroidBindingContext> CreateContextStack()
        {
            return new MvxAndroidBindingContextStack();
        }

        protected virtual void InitializeViewTypeResolver(IMvxIoCProvider iocProvider)
        {
            var typeCache = CreateViewTypeCache();
            iocProvider.RegisterSingleton<IMvxTypeCache<View>>(typeCache);

            var fullNameViewTypeResolver = new MvxAxmlNameViewTypeResolver(typeCache);
            iocProvider.RegisterSingleton<IMvxAxmlNameViewTypeResolver>(fullNameViewTypeResolver);
            var listViewTypeResolver = new MvxNamespaceListViewTypeResolver(typeCache);
            iocProvider.RegisterSingleton<IMvxNamespaceListViewTypeResolver>(listViewTypeResolver);
            var justNameTypeResolver = new MvxJustNameViewTypeResolver(typeCache);

            var composite = new MvxCompositeViewTypeResolver(fullNameViewTypeResolver, listViewTypeResolver, justNameTypeResolver);
            var cached = new MvxCachedViewTypeResolver(composite);
            iocProvider.RegisterSingleton<IMvxViewTypeResolver>(cached);
        }

        protected virtual IMvxTypeCache<View> CreateViewTypeCache()
        {
            return new MvxTypeCache<View>();
        }
    }
}
