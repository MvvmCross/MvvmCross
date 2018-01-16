using System;
using Android;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [Register(nameof(SplitDetailView))]
    public class SplitDetailView : MvxFragment<SplitDetailViewModel>
    {
        public SplitRootView BaseActivity => (SplitRootView)Activity;

        private Toolbar _toolbar;
        private MvxActionBarDrawerToggle _drawerToggle;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SplitDetailView, null);

            _toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (_toolbar != null)
            {
                BaseActivity.SetSupportActionBar(_toolbar);
                BaseActivity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);

                _drawerToggle = new MvxActionBarDrawerToggle(
                    Activity,                               // host Activity
                    BaseActivity.DrawerLayout,  // DrawerLayout object
                    _toolbar,                               // nav drawer icon to replace 'Up' caret
                    Resource.String.drawer_open,            // "open drawer" description
                    Resource.String.drawer_close            // "close drawer" description
                );
                BaseActivity.DrawerLayout.AddDrawerListener(_drawerToggle);
                _drawerToggle.DrawerIndicatorEnabled = true;
            }

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
