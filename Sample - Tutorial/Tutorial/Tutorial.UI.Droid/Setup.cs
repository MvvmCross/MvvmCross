using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Droid;
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

        protected override MvxApplication CreateApp()
        {
            return new App();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] {typeof (Converters)}; }
        }
    }
}