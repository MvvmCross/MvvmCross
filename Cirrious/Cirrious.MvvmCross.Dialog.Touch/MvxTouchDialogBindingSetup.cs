// MvxTouchDialogBindingSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Target;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using CrossUI.Touch.Dialog.Elements;

namespace Cirrious.MvvmCross.Dialog.Touch
{
    public abstract class MvxTouchDialogBindingSetup
        : MvxBaseTouchBindingSetup
    {
        protected MvxTouchDialogBindingSetup(MvxApplicationDelegate applicationDelegate,
                                             IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(
                                         typeof (MvxEntryElementValueBinding), typeof (EntryElement), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(
                                         typeof (MvxValueElementValueBinding), typeof (ValueElement), "Value"));
            /*
             * these methods no longer used - https://github.com/slodge/MvvmCross/issues/26
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<float>), typeof(ValueElement<float>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<DateTime>), typeof(ValueElement<DateTime>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<bool>), typeof(ValueElement<bool>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<UIImage>), typeof(ValueElement<UIImage>), "Value"));
             */
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof (MvxRadioRootElementBinding),
                                                                                   typeof (RootElement), "RadioSelected"));
        }
    }
}