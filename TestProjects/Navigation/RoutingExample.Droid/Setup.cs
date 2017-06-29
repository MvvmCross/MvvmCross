using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using RoutingExample.Core;

namespace RoutingExample.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MyViewPresenter(AndroidViewAssemblies);
        }
    }

    public class MyViewPresenter : MvxAndroidPresenter
    {
        public MyViewPresenter(IEnumerable<Assembly> AndroidViewAssemblies) : base(AndroidViewAssemblies)
        {
        }
    }
}