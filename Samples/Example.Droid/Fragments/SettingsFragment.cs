using Android.Runtime;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using Example.Core.ViewModels;

namespace Example.Droid.Fragments
{
    [MvxFragment]
    [Register("example.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;
    }
}