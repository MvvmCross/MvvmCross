// MvxAppCompatSetupHelper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Android.Support.V7.Widget;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Droid.Support.AppCompat.Target;
using Cirrious.MvvmCross.Droid.Support.AppCompat.Widget;

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
    }
}