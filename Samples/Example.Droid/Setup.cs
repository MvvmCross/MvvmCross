using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.Droid.Support.Fragging.Presenter;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Reflection;

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

        protected override IList<Assembly> AndroidViewAssemblies
        {
            get
            {
                var assemblies = base.AndroidViewAssemblies;
                assemblies.Add(typeof(Android.Support.Design.Widget.NavigationView).Assembly);
                assemblies.Add(typeof(Android.Support.Design.Widget.FloatingActionButton).Assembly);
                assemblies.Add(typeof(Android.Support.V7.Widget.Toolbar).Assembly);
                assemblies.Add(typeof(Android.Support.V4.Widget.DrawerLayout).Assembly);
                assemblies.Add(typeof(Android.Support.V4.View.ViewPager).Assembly);
                assemblies.Add(typeof(Cirrious.MvvmCross.Droid.Support.RecyclerView.MvxRecyclerView).Assembly);

                return assemblies;
            }
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var customPresenter = new MvxFragmentsPresenter();
            Mvx.RegisterSingleton<IMvxFragmentsPresenter>(customPresenter);
            return customPresenter;
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}