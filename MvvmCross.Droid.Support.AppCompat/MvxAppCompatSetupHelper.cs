// MvxAppCompatSetupHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Widget;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Support.AppCompat.Target;
using Cirrious.MvvmCross.Droid.Support.AppCompat.Widget;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace Cirrious.MvvmCross.Droid.Support.AppCompat
{
    public static class MvxAppCompatSetupHelper
    {
        public static void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<MvxAppCompatSpinner>("SelectedItem",
                spinner => new MvxAppCompatSpinnerSelectedItemBinding(spinner));
            registry.RegisterCustomBindingFactory<SearchView>(
                "Query",
                search => new MvxSearchViewQueryTextTargetBinding(search)
                );
        }

        public static void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(SearchView), "Query");
        }
    }
}