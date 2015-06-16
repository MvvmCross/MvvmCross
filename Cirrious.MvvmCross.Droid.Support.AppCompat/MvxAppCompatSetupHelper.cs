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
        }
    }
}