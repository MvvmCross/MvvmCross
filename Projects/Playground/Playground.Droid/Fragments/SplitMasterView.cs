// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Google.Android.Material.Navigation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using Playground.Core.ViewModels;
using Playground.Droid.Activities;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_navigation_frame)]
    [Register(nameof(SplitMasterView))]
    public class SplitMasterView : MvxFragment<SplitMasterViewModel>, NavigationView.IOnNavigationItemSelectedListener
    {
        private IMenuItem previousMenuItem;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SplitMasterView, null);

            return view;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            item.SetCheckable(true);
            item.SetChecked(true);
            previousMenuItem?.SetChecked(false);
            previousMenuItem = item;

            Navigate(item.ItemId);

            return true;
        }

        private Task Navigate(int itemId)
        {
            ((SplitRootView)Activity).DrawerLayout.CloseDrawers();
            return Task.Delay(TimeSpan.FromMilliseconds(250));

            //switch (itemId)
            //{
            //    case Resource.Id.nav_home:
            //        break;
            //}
        }
    }
}
