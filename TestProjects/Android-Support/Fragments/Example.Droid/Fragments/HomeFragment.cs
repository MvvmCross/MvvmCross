﻿using Android.Runtime;
using Example.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame, true)]
	[MvxFragment(typeof(SecondHostViewModel), Resource.Id.content_frame, true)]
    [Register("example.droid.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_home;
    }
}