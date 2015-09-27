using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Transitions;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Support.Fragging.Presenter;
using Cirrious.MvvmCross.ViewModels;
using Example.Core.ViewModels;
using Example.Droid.Fragments;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

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

            RegisterForDetailsRequests(bundle);

            SetContentView(Resource.Layout.activity_main);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if(bundle == null)
                ViewModel.ShowMenu();
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

        private void RegisterForDetailsRequests(Bundle bundle)
        {
            RegisterFragment<MenuFragment, MenuViewModel>(typeof(MenuViewModel).Name, bundle);
            RegisterFragment<ExamplesFragment, ExamplesViewModel>(typeof(ExamplesViewModel).Name, bundle);
            RegisterFragment<SettingsFragment, SettingsViewModel>(typeof(SettingsViewModel).Name, bundle);
        }

        protected override IMvxCachedFragmentInfo CreateFragmentInfo<TFragment, TViewModel>(string tag)
        {
            var fragInfo = myFragmentsInfo[tag];

            if(tag != typeof(MenuViewModel).Name)
                fragInfo.TransitionInfo = fragTransitions;

            return fragInfo;
        }

        public override void OnFragmentCreated(IMvxCachedFragmentInfo fragmentInfo, FragmentTransaction transaction)
        {
            var myCustomInfo = (CustomFragmentInfo) fragmentInfo;
            InflateTransitions(myCustomInfo);
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

        private void InflateTransitions(CustomFragmentInfo fragmentInfo)
        {
            var frag = fragmentInfo.CachedFragment;
            var transitionInfo = fragmentInfo.TransitionInfo;

            if (transitionInfo == null)
                return;

            var transitionInflater = TransitionInflater.From(this);
            frag.EnterTransition = transitionInflater.InflateTransition(transitionInfo.EnterTransitionId);
            frag.ExitTransition = transitionInflater.InflateTransition(transitionInfo.ExitTransitionId);
        }

        public void RegisterFragment<TFragment, TViewModel>(string tag, Bundle args)
            where TFragment : IMvxFragmentView
            where TViewModel : IMvxViewModel
        {
            var customPresenter = Mvx.Resolve<IMvxFragmentsPresenter>();
            customPresenter.RegisterViewModelAtHost<TViewModel>(this);
            RegisterFragment<TFragment, TViewModel>(tag);
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

        private static Dictionary<string, CustomFragmentInfo> myFragmentsInfo = new Dictionary<string, CustomFragmentInfo>()
        {
            {typeof(MenuViewModel).Name, new CustomFragmentInfo(typeof(MenuViewModel).Name, typeof(MenuFragment), typeof(MenuViewModel),false)},
            {typeof(ExamplesViewModel).Name, new CustomFragmentInfo( typeof(ExamplesViewModel).Name, typeof(ExamplesFragment), typeof(ExamplesViewModel), true, isRoot: true)},
            {typeof(SettingsViewModel).Name, new CustomFragmentInfo( typeof(SettingsViewModel).Name, typeof(SettingsFragment), typeof(SettingsViewModel), true, isRoot: true)}
        };

        private static FragmentTransitionInfo fragTransitions = new FragmentTransitionInfo()
        {
            EnterTransitionId = Resource.Transition.slide_left,
            ExitTransitionId = Resource.Transition.slide_right,
        };
    }

    public class CustomFragmentInfo : MvxCachedFragmentInfo
    {
        public FragmentTransitionInfo TransitionInfo { get; set; }
        public bool IsRoot { get; set; }

        public CustomFragmentInfo(string tag, Type fragmentType, Type viewModelType, bool addToBackstack, FragmentTransitionInfo transitionInfo = null, bool isRoot = false)
            : base(tag, fragmentType, viewModelType)
        {
            AddToBackStack = addToBackstack;
            TransitionInfo = transitionInfo;
            IsRoot = isRoot;
        }

    }

    public class FragmentTransitionInfo
    {
        public int EnterTransitionId;
        public int ExitTransitionId;
        public int ReenterTransitionId;
        public int ReturnTransitionId;
    }

}