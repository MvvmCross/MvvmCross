using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Cirrious.MvvmCross.Droid.Support.Fragging;
using Cirrious.MvvmCross.Droid.Support.V4;
using Example.Core.ViewModels;

namespace Example.Droid.Fragments
{
    [MvxFragment]
    [Register("example.droid.fragments.ExampleViewPagerFragment")]
    public class ExampleViewPagerFragment : BaseFragment<ExampleViewPagerViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentStatePagerAdapter.FragmentInfo>
                {
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView 1", typeof(RecyclerViewFragment), typeof(RecyclerViewModel)),
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView 2", typeof(RecyclerViewFragment), typeof(RecyclerViewModel)),
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView 3", typeof(RecyclerViewFragment), typeof(RecyclerViewModel)),
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView 4", typeof(RecyclerViewFragment), typeof(RecyclerViewModel)),
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView 5", typeof(RecyclerViewFragment), typeof(RecyclerViewModel))
                };
                viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }

        protected override int FragmentId => Resource.Layout.fragment_example_viewpager;
    }
}