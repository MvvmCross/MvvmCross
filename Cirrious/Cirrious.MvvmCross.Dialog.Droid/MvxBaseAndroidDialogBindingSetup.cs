#region Copyright

// <copyright file="MvxBaseAndroidDialogBindingSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.Content;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Dialog.Droid.Target;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using CrossUI.Droid.Dialog.Elements;

namespace Cirrious.MvvmCross.Dialog.Droid
{
    public abstract class MvxBaseAndroidDialogBindingSetup
        : MvxBaseAndroidBindingSetup, IMvxServiceProducer
    {
        protected MvxBaseAndroidDialogBindingSetup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override void InitializeLastChance()
        {
            InitializeDialogBinding();
            InitializeUserInterfaceBuilder();
            base.InitializeLastChance();
        }

        private void InitializeUserInterfaceBuilder()
        {
            // TODO...
        }

        protected virtual void InitializeDialogBinding()
        {
#warning How to intialise DroidResources?!
            //DroidResources.Initialise(typeof(Resource.Layout));
        }

        protected override void FillTargetFactories(
            Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxPropertyInfoTargetBindingFactory(typeof (ValueElement), "Value",
                                                                             (element, propertyInfo) =>
                                                                             new MvxElementValueTargetBinding(element,
                                                                                                              propertyInfo)));
            base.FillTargetFactories(registry);
        }
    }
}