// MvxAppCompatSetupHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Support.V7.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Support.V7.AppCompat.Target;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public static class MvxAppCompatSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding),
                typeof(MvxAppCompatAutoCompleteTextView),
                MvxAppCompatPropertyBinding.MvxAppCompatAutoCompleteTextView_PartialText);

            registry.RegisterPropertyInfoBindingFactory(
                typeof(MvxAppCompatAutoCompleteTextViewSelectedObjectTargetBinding),
                typeof(MvxAppCompatAutoCompleteTextView),
                MvxAppCompatPropertyBinding.MvxAppCompatAutoCompleteTextView_SelectedObject);

            registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>(
                MvxAppCompatPropertyBinding.MvxAppCompatSpinner_SelectedItem,
                spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));

            registry.RegisterCustomBindingFactory<MvxAppCompatRadioGroup>(
                MvxAppCompatPropertyBinding.MvxAppCompatRadioGroup_SelectedItem,
                radioGroup => new MvxAppCompatRadioGroupSelectedItemBinding(radioGroup));

            registry.RegisterCustomBindingFactory<SearchView>(
                MvxAppCompatPropertyBinding.SearchView_Query,
                search => new MvxAppCompatSearchViewQueryTextTargetBinding(search));

            registry.RegisterCustomBindingFactory<Toolbar>(
                MvxAppCompatPropertyBinding.Toolbar_Subtitle,
                toolbar => new MvxToolbarSubtitleBinding(toolbar));
        }

        public static void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(SearchView), MvxAppCompatPropertyBinding.SearchView_Query);
            registry.AddOrOverwrite(typeof(MvxAppCompatImageView), nameof(MvxAppCompatImageView.ImageUrl));

        }
    }
}
