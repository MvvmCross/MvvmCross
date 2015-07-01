using System.Collections.Generic;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Support.V4;
using Example.Core.ViewModels;
using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
    [MvxOwnedViewModelFragment]
    [Register("example.droid.fragments.ExamplesFragment")]
    public class ExamplesFragment : MvxFragment<ExamplesViewModel>
    {
        private Toolbar _toolbar;
        private MvxActionBarDrawerToggle _drawerToggle;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_examples, null);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                ((MainActivity)Activity).SetSupportActionBar(_toolbar);
                ((MainActivity)Activity).SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                               // host Activity
                    ((MainActivity)Activity).DrawerLayout,  // DrawerLayout object
                    _toolbar,                               // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open,            // "open drawer" description
                    Resource.String.drawer_close            // "close drawer" description
                );

                ((MainActivity)Activity).DrawerLayout.SetDrawerListener(_drawerToggle);
            }

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentStatePagerAdapter.FragmentInfo>
                {
                    new MvxFragmentStatePagerAdapter.FragmentInfo
                    {
                        FragmentType = typeof(RecyclerViewFragment),
                        Title = "RecyclerView",
                        ViewModel = ViewModel.Recycler
                    }
                };
                viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (_toolbar != null)
                _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            if (_toolbar != null)
                _drawerToggle.SyncState();
        }
    }
}