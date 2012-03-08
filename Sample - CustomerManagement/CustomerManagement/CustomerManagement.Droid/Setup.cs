using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.MvvmCross.Android.Platform;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Android;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using CustomerManagement.Core;
using CustomerManagement.Core.ViewModels;
using CustomerManagement.Droid.Views;

namespace CustomerManagement.Droid
{
    public class Setup 
        : MvxBaseAndroidBindingSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override MvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }
    }
}