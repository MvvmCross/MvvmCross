using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Example.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.V4;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Example.Droid.Activities;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Android.Views;
using Android.OS;
using Android.Runtime;
using System.Collections.Generic;

namespace Example.Droid.Fragments
{
    [Register("example.droid.fragments.ExamplesFragment")]
    public class ExamplesFragment : MvxFragment<ExamplesViewModel>
    {
        private Toolbar toolbar;
        private MvxActionBarDrawerToggle drawerToggle;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_examples, null);

            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                ((MainActivity)Activity).SetSupportActionBar(toolbar);
                ((MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                  /* host Activity */
                    ((MainActivity)Activity).drawerLayout,         /* DrawerLayout object */
                    toolbar,  /* nav drawer icon to replace 'Up' caret */
                    Resource.String.drawer_open,  /* "open drawer" description */
                    Resource.String.drawer_close  /* "close drawer" description */
                );

                ((MainActivity)Activity).drawerLayout.SetDrawerListener(drawerToggle);
            }

            var viewPager = view.FindViewById<Android.Support.V4.View.ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentStatePagerAdapter.FragmentInfo>
                {
                    new MvxFragmentStatePagerAdapter.FragmentInfo
                    {
                        FragmentType = typeof(RecyclerViewFragment),
                        Title = "RecyclerView",
                        ViewModel = new RecyclerViewModel()
                    }
                };
                viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (toolbar != null)
                drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (toolbar != null)
                drawerToggle.SyncState();
        }
    }
}

