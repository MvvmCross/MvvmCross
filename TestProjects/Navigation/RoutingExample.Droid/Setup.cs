using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Support.V4.App;
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

    public class MyViewPresenter : MvxAndroidViewPresenter
    {
        public MyViewPresenter(IEnumerable<Assembly> AndroidViewAssemblies) : base(AndroidViewAssemblies)
        {
        }

        protected override IMvxFragmentView CreateFragment(System.Type fragType, Android.OS.Bundle bundle)
        {
            return Fragment.Instantiate(Activity, FragmentJavaName(fragType),
                    bundle) as IMvxFragmentView;
        }

        protected override void ReplaceFragment(MvxFragmentAttribute mvxFragmentAttributeAssociated, IMvxFragmentView fragment, string fragmentTag)
        {
            var ft = (Activity as MvxCachingFragmentCompatActivity).SupportFragmentManager.BeginTransaction();
            ft.Replace(mvxFragmentAttributeAssociated.FragmentContentId, fragment as Fragment, fragmentTag);
        }
    }
}