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
    [Register("example.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId {
            get {
                return Resource.Layout.fragment_settings;
            }
        }
    }
}