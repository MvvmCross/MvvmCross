using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;

namespace Example.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

		protected override IEnumerable<Assembly> AndroidViewAssemblies => new List<Assembly>(base.AndroidViewAssemblies)
		{
			typeof(Android.Support.Design.Widget.NavigationView).Assembly,
			typeof(Android.Support.Design.Widget.FloatingActionButton).Assembly,
			typeof(Android.Support.V7.Widget.Toolbar).Assembly,
			typeof(Android.Support.V4.Widget.DrawerLayout).Assembly,
			typeof(Android.Support.V4.View.ViewPager).Assembly,
			typeof(MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView).Assembly
		};

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var customPresenter = new MvxFragmentsPresenter();
            Mvx.RegisterSingleton<IMvxFragmentsPresenter>(customPresenter);

			var fragments = CreatableTypes().WithAttribute<MvxFragmentAttribute>();
			customPresenter.RegisterFragments(fragments);

            return customPresenter;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}