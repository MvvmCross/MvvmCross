using Cirrious.MvvmCross.Binding.Bindings.Source.Construction;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Binding.Touch
{
    public class MvxTouchBindingSetup 
        : MvxBindingSetup
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // TODO - are there any special targets for Touch?

            base.FillTargetFactories(registry);
        }

        protected override void RegisterPlatformSpecificComponents()
        {
            // TODO - are there any special platform specific components for Touch?

            base.RegisterPlatformSpecificComponents();
        }
    }
}