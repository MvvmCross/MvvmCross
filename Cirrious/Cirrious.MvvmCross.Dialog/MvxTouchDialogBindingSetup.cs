#region Copyright
// <copyright file="MvxTouchDialogBindingSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Touch;
using Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements;
using Cirrious.MvvmCross.Dialog.Touch.Target;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Touch.Platform;
using MonoTouch.UIKit;

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
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<float>), typeof(ValueElement<float>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<DateTime>), typeof(ValueElement<DateTime>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<bool>), typeof(ValueElement<bool>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxValueElementValueBinding<UIImage>), typeof(ValueElement<UIImage>), "Value"));
            registry.RegisterFactory(new MvxSimplePropertyInfoTargetBindingFactory(typeof(MvxRadioRootElementBinding), typeof(RootElement), "RadioSelected"));
        }
    }
}