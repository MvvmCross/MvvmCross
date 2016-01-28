// MvxAppCompatSetupHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat.Target;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    using Android.Support.V4.Widget;
    using Android.Support.V7.Widget;
    using Android.Widget;

    public static class MvxAppCompatSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding),
                typeof(AppCompatAutoCompleteTextView), "PartialText");
            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(AppCompatAutoCompleteTextView),
                "SelectedObject");

            registry.RegisterCustomBindingFactory<AppCompatImageView>(
                "Bitmap",
                imageView => new MvxAppCompatImageViewBitmapTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(
                "DrawableId",
                imageView => new MvxAppCompatImageViewDrawableTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(
                "DrawableName",
                imageView => new MvxAppCompatImageViewDrawableNameTargetBinding(imageView));
            registry.RegisterCustomBindingFactory<AppCompatImageView>(
                "AssetImagePath",
                imageView => new MvxAppCompatImageViewImageTargetBinding(imageView));

            registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>(
                "SelectedItem",
                spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<MvxAppCompatListView>(
                "SelectedItem",
                adapterView => new MvxAppCompatListViewSelectedItemTargetBinding(adapterView));

            registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                "SelectedItem",
                radioGroup => new MvxAppCompatRadioGroupSelectedItemBinding(radioGroup));
            registry.RegisterCustomBindingFactory<SearchViewCompat>(
                "Query",
                search => new MvxAppCompatSearchViewQueryTextTargetBinding(search)
                );
        }

        public static void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(SearchViewCompat), "Query");
            registry.AddOrOverwrite(typeof(MvxAppCompatListView), "ItemsSource");
            registry.AddOrOverwrite(typeof(MvxAppCompatImageView), "ImageUrl");

        }
    }
}