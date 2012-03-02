using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Target;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using MonoTouch.Dialog;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public abstract class MvxTouchDialogBindingSetup
        : MvxBaseTouchBindingSetup
    {
        protected MvxTouchDialogBindingSetup(MvxApplicationDelegate applicationDelegate, IMvxTouchViewPresenter presenter) 
            : base(applicationDelegate, presenter)
        {
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxEntryElementValueBinding), typeof(EntryElement), "Value"));
        }
    }
}