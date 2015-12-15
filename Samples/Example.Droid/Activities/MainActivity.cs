using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Views;
using Cirrious.CrossCore;
using MvvmCross.Droid.Support.V7.Fragging;
using MvvmCross.Droid.Support.V7.Fragging.Fragments;
using MvvmCross.Droid.Support.V7.Fragging.Presenter;
using Cirrious.MvvmCross.ViewModels;
using Example.Core.ViewModels;
using Example.Droid.Activities.Caching;
using Example.Droid.Fragments;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.Fragging.Caching;

namespace Example.Droid.Activities
{
    [Activity(
        Label = "Examples",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        Name = "example.droid.activities.MainActivity"
    )]
    public class MainActivity : MvxCachingFragmentCompatActivity<MainViewModel>, IMvxFragmentHost
    {
        public DrawerLayout DrawerLayout;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            RegisterForDetailsRequests();

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if(bundle == null)
                ViewModel.ShowMenuAndFirstDetail();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    DrawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override IFragmentCacheConfiguration BuildFragmentCacheConfiguration()
        {
            return new FragmentCacheConfigurationCustomFragmentInfo();
        }

        private void RegisterForDetailsRequests()
        {
            //TODO improve Mvx and MvxAndroidSupport to enable registration based on a config dictionary.
            //currently, all the register methods expect the Frag + VM types to be added as generics, thus, we need to change it to support parameters
            RegisterFragmentAtHost<MenuFragment, MenuViewModel>(typeof(MenuViewModel).Name);
            RegisterFragmentAtHost<HomeFragment, HomeViewModel>(typeof(HomeViewModel).Name);
            RegisterFragmentAtHost<ExampleViewPagerFragment, ExampleViewPagerViewModel>(typeof(ExampleViewPagerViewModel).Name);
            RegisterFragmentAtHost<ExampleRecyclerViewFragment, ExampleRecyclerViewModel>(typeof(ExampleRecyclerViewModel).Name);
            RegisterFragmentAtHost<SettingsFragment, SettingsViewModel>(typeof(SettingsViewModel).Name);

            (FragmentCacheConfiguration as FragmentCacheConfigurationCustomFragmentInfo).RegisterFragmentsToCache();
        }
        public void RegisterFragmentAtHost<TFragment, TViewModel>(string tag)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            var customPresenter = Mvx.Resolve<IMvxFragmentsPresenter>();
            customPresenter.RegisterViewModelAtHost<TViewModel>(this);
        }

        public override void OnFragmentCreated(IMvxCachedFragmentInfo fragmentInfo, Android.Support.V4.App.FragmentTransaction transaction)
        {
            base.OnFragmentCreated(fragmentInfo, transaction);

            var myCustomInfo = (CustomFragmentInfo)fragmentInfo;

            // You can do fragment + transaction based configurations here.
            // Note that, the cached fragment might be reused in another transaction afterwards.
        }

        private void CheckIfMenuIsNeeded(CustomFragmentInfo myCustomInfo)
        {
            //If not root, we will block the menu sliding gesture and show the back button on top
            if (myCustomInfo.IsRoot)
                ShowHamburguerMenu();
            else
                ShowBackButton();
        }

        private void ShowBackButton()
        {
            //TODO Tell the toggle to set the indicator off
            //this.DrawerToggle.DrawerIndicatorEnabled = false;
            
            //Block the menu slide gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeLockedClosed);
        }

        private void ShowHamburguerMenu()
        {
            //TODO set toggle indicator as enabled 
            //this.DrawerToggle.DrawerIndicatorEnabled = true;

            //Unlock the menu sliding gesture
            DrawerLayout.SetDrawerLockMode(DrawerLayout.LockModeUnlocked);
        }



        public bool Show(MvxViewModelRequest request, Bundle bundle)
        {
            if (request.ViewModelType == typeof(MenuViewModel))
            {
                ShowFragment(request.ViewModelType.Name, Resource.Id.navigation_frame, bundle);
                return true;
            }
            else
            {
                ShowFragment(request.ViewModelType.Name, Resource.Id.content_frame, bundle);
                return true;
            }
        }

        public override void OnFragmentChanged(IMvxCachedFragmentInfo fragmentInfo)
        {
            var myCustomInfo = (CustomFragmentInfo)fragmentInfo;
            CheckIfMenuIsNeeded(myCustomInfo);
        }

        public bool Close (IMvxViewModel viewModel)
        {
            CloseFragment (viewModel.GetType ().Name, Resource.Id.content_frame);
            return true;
        }

        public override void OnBackPressed()
        {
            if (DrawerLayout != null && DrawerLayout.IsDrawerOpen(GravityCompat.Start))
                DrawerLayout.CloseDrawers();
            else
                base.OnBackPressed();
        }

         
    }

    public class CustomFragmentInfo : MvxCachedFragmentInfo
    {
        public bool IsRoot { get; set; }

        public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack = false, bool isRoot = false)
            : base(tag, fragmentType, viewModelType, addToBackstack)
        {
            IsRoot = isRoot;
        }

    }
}