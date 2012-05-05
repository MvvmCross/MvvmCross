using System;
using System.Collections.Generic;
using Android.Content;
using Cirrious.MvvmCross.Application;
using Cirrious.MvvmCross.Binding.Droid;
using TwitterSearch.Core;
using TwitterSearch.Core.Converters;

namespace TwitterSearch.UI.Droid
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
            return new TwitterSearchApp();
        }

        protected override IEnumerable<Type> ValueConverterHolders
        {
            get { return new[] { typeof(Converters) }; }
        }
    }
}

