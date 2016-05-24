using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Shared.Presenter;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
	public abstract class MvxAppCompatSetup : MvxAndroidSetup
	{
		protected MvxAppCompatSetup(Context applicationContext)
            : base(applicationContext)
        {
		}

		protected abstract override IMvxApplication CreateApp();

		protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(Android.Support.V7.Widget.Toolbar).Assembly,
			typeof(Android.Support.V4.Widget.DrawerLayout).Assembly,
         	typeof(Android.Support.V4.Widget.NestedScrollView).Assembly,
			typeof(Android.Support.V4.Widget.SlidingPaneLayout).Assembly,
			typeof(Android.Support.V4.View.ViewPager).Assembly,
			typeof(MvvmCross.Droid.Support.V4.MvxSwipeRefreshLayout).Assembly,
		};

		/// <summary>
		/// This is very important to override. The default view presenter does not know how to show fragments!
		/// </summary>
		protected override IMvxAndroidViewPresenter CreateViewPresenter()
		{
			var mvxFragmentsPresenter = new MvxFragmentsPresenter(AndroidViewAssemblies);
			Mvx.RegisterSingleton<IMvxAndroidViewPresenter>(mvxFragmentsPresenter);
			return mvxFragmentsPresenter;
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

