using Android.Runtime;
using Example.Core.ViewModels;
using Example.Droid.Activities;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace Example.Droid.Fragments
{
	[MvxFragment(
		ParentType = typeof(MainActivity)
	)]
    [Register("example.droid.fragments.HomeFragment")]
    public class HomeFragment : BaseFragment<HomeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_home;
    }
}