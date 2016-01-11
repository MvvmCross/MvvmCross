using Android.Content;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Droid;
using MvvmCross.Droid.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxAppCompatAndroidSetup : MvxAndroidSetup
    {
        protected MvxAppCompatAndroidSetup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override MvxAndroidBindingBuilder CreateBindingBuilder()
        {
            return new MvxAppCompatBindingBuilder();
        }
    }
}
