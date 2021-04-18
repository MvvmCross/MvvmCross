// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
{
    [MvxBottomNavigationViewPresentation("Tab 3", Resource.Id.viewpager, Resource.Id.navigation, ActivityHostViewModelType = typeof(BottomTabsRootViewModel), FragmentContentId = Resource.Id.content_frame)]
    [Register(nameof(BottomTab3View))]
    public class BottomTab3View : MvxFragment<BottomTab3ViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.BottomTab3View, null);

            return view;
        }
    }
}
