using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Binding.Droid;
using Cirrious.MvvmCross.Plugins.Json;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using Tutorial.Core;
using Tutorial.Core.Converters;

namespace Tutorial.UI.Droid
{
    public class Setup
        : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }
    }
}