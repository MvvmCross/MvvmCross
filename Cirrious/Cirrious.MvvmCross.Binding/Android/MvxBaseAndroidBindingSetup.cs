using Android.Content;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Binding.Interfaces.Binders;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;

namespace Cirrious.MvvmCross.Binding.Android
{
    public abstract class MvxBaseAndroidBindingSetup
        : MvxBaseAndroidSetup
    {
        protected MvxBaseAndroidBindingSetup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override void InitializeLastChance()
        {
            var bindingBuilder = new MvxAndroidBindingBuilder(FillTargetFactories, FillValueConverters);
            bindingBuilder.DoRegistration();

            base.InitializeLastChance();
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            // nothing to do in this base class
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // nothing to do in this base class
        }
    }
}