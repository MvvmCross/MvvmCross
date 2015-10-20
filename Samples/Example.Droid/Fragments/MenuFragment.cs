using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Example.Core.ViewModels;
using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
    [MvxOwnedViewModelFragment]
    [Register("example.droid.fragments.MenuFragment")]
    public class MenuFragment : MvxFragment<MenuViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private NavigationView navigationView;
        private IMenuItem previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_navigation, null);

            navigationView = view.FindViewById<NavigationView>(Resource.Id.navigation_view);
            navigationView.SetNavigationItemSelectedListener(this);
            navigationView.Menu.FindItem(Resource.Id.nav_home).SetChecked(true);

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            if (previousMenuItem != null) {
                previousMenuItem.SetChecked(false);
            }
            previousMenuItem = item;

            Navigate (item.ItemId);

            return true;
        }

        private async Task Navigate(int itemId)
        {
            ((MainActivity)Activity).DrawerLayout.CloseDrawers ();
            await Task.Delay (TimeSpan.FromMilliseconds (250));

            switch (itemId) {
            case Resource.Id.nav_home:
                ViewModel.ShowViewModelAndroid(typeof(HomeViewModel));
                break;
            case Resource.Id.nav_viewpager:
                ViewModel.ShowViewModelAndroid(typeof(ExampleViewPagerViewModel));
                break;
            case Resource.Id.nav_recyclerview:
                ViewModel.ShowViewModelAndroid(typeof(ExampleRecyclerViewModel));
                break;
            case Resource.Id.nav_settings:
                ViewModel.ShowViewModelAndroid(typeof(SettingsViewModel));
                break;
            case Resource.Id.nav_helpfeedback:
                ViewModel.ShowViewModelAndroid(typeof(SettingsViewModel));
                break;
            }
        }
    }
}