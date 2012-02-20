using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Target;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public class MvxTouchDialogBindingSetup 
        : MvxTouchBindingSetup
    {
        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxEntryElementValueBinding), typeof(EntryElement), "Value"));
        }
    }
}