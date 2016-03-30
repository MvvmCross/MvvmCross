using Android.Runtime;
using MvvmCross.Droid.Support.V7.Fragging;
using Example.Core.ViewModels;
using MvvmCross.Droid.Shared.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("example.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;
    }
}