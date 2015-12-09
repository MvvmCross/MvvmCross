// MvxTouchDialogSetup.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch
{
    using CrossUI.Touch.Dialog.Elements;

    using MvvmCross.Binding.Bindings.Target.Construction;
    using MvvmCross.Dialog.Touch.Target;

    using UIKit;

    public abstract class MvxTouchDialogSetup
        : MvxTouchSetup
    {
        protected MvxTouchDialogSetup(IMvxApplicationDelegate applicationDelegate,
                                      IMvxTouchViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
        }

        protected MvxTouchDialogSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(
                                         typeof(MvxEntryElementValueBinding), typeof(EntryElement), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(
                                         typeof(MvxValueElementValueBinding), typeof(ValueElement), "Value"));
            /*
             * these methods no longer used - https://github.com/slodge/MvvmCross/issues/26
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<float>), typeof(ValueElement<float>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<DateTime>), typeof(ValueElement<DateTime>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<bool>), typeof(ValueElement<bool>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<UIImage>), typeof(ValueElement<UIImage>), "Value"));
             */
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementBinding),
                                                                                   typeof(RootElement), "RadioSelected"));
        }

        protected override void FillBindingNames(Binding.BindingContext.IMvxBindingNameRegistry registry)
        {
            registry.AddOrOverwrite(typeof(ValueElement), "Value");
            base.FillBindingNames(registry);
        }
    }
}