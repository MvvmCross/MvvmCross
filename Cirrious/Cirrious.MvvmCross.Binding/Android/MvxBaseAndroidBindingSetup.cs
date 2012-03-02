#region Copyright
// <copyright file="MvxBaseAndroidBindingSetup.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

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