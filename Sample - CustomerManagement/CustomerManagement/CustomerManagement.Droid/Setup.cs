using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Dialog.Droid;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using CustomerManagement.Core;
using CustomerManagement.Core.Interfaces;
using CustomerManagement.Core.ViewModels;

namespace CustomerManagement.Droid
{
    public class Setup 
        : MvxAndroidDialogSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            var app =  new App();
            return app;
        }

        protected override void InitializeLastChance()
        {
            var closer = new SimpleDroidViewModelCloser();
            Mvx.RegisterSingleton<IViewModelCloser>(closer);
            base.InitializeLastChance();
        }
    }
}