using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Android;
using Tutorial.Core;
using Tutorial.Core.Converters;

namespace Tutorial.UI.Droid
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
            return new App();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] {typeof (Converters)}; }
        }
    }
}