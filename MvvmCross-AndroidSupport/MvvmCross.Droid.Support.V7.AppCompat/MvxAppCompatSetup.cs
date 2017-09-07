﻿using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxAppCompatSetup : MvxAndroidSetup
    {
        protected MvxAppCompatSetup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies => 
            new List<Assembly>(base.AndroidViewAssemblies)
            {
                typeof(Toolbar).Assembly,
                typeof(DrawerLayout).Assembly,
                typeof(MvxSwipeRefreshLayout).Assembly
            };

        /// <summary>
        /// This is very important to override. The default view presenter does not know how to show fragments!
        /// </summary>
        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new MvxAppCompatViewPresenter(AndroidViewAssemblies);
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }

        protected override void FillBindingNames(IMvxBindingNameRegistry registry)
        {
            MvxAppCompatSetupHelper.FillDefaultBindingNames(registry);
            base.FillBindingNames(registry);
        }
    }
}