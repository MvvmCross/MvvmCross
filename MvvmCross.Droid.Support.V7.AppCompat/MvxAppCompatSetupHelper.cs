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
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
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

            registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>(
                "SelectedItem",
                spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                "SelectedItem",
                radioGroup => new MvxAppCompatRadioGroupSelectedItemBinding(radioGroup));
            registry.RegisterCustomBindingFactory<SearchViewCompat>(
                "Query",
                search => new MvxAppCompatSearchViewQueryTextTargetBinding(search));

            registry.RegisterCustomBindingFactory<Android.Support.V7.Widget.Toolbar>(
                "Subtitle",
                toolbar => new MvxToolbarSubtitleBinding(toolbar));
        }

        public static void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(SearchViewCompat), "Query");
            registry.AddOrOverwrite(typeof(MvxAppCompatImageView), "ImageUrl");

        }
    }
}