// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AndroidX.AppCompat.Widget;
using Google.Android.Material.FloatingActionButton;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.DroidX.AppCompat.Target;
using MvvmCross.DroidX.AppCompat.Widget;

namespace MvvmCross.DroidX.AppCompat
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
            registry.AddOrOverwrite(typeof(FloatingActionButton), MvxAppCompatPropertyBinding.FloatingActionButton_Click);
        }
    }
}
