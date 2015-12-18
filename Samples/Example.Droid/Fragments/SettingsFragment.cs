using Android.Runtime;
using MvvmCross.Droid.Support.V7.Fragging;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;
using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
	[MvxFragment(
		ParentType = typeof(MainActivity)
	)]
    [Register("example.droid.fragments.SettingsFragment")]
    public class SettingsFragment : BaseFragment<SettingsViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_settings;
    }
}