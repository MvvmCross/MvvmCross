using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Core;
using CustomerManagement.Core.ViewModels;
using CustomerManagement.Droid.Views;
using FooBar.Dialog.Droid;

namespace CustomerManagement.Droid
{
    public class Setup 
        : MvxBaseAndroidBindingSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override void FillTargetFactories(Cirrious.MvvmCross.Binding.Interfaces.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxPropertyInfoTargetBindingFactory(typeof(ValueElement), "Value", (element, propertyInfo) => new MvxElementValueTargetBinding(element, propertyInfo)));
            base.FillTargetFactories(registry);
        }

        protected override MvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }
    }
}