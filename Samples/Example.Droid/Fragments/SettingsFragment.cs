using Android.Runtime;
using MvvmCross.Droid.Support.V7.Fragging;
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