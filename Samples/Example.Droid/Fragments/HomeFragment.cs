using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using MvvmCross.Droid.Support.V7.Fragging;
using MvvmCross.Droid.Support.V4;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragment]
    [Register("example.droid.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_home;
    }
}