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