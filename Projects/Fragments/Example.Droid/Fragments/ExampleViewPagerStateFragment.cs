using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Example.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Droid.Support.V4;

namespace Example.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("example.droid.fragments.ExampleViewPagerStateFragment")]
    public class ExampleViewPagerStateFragment : BaseStateFragment<ExampleViewPagerStateViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_example_viewpager_state;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
				var fragments = new List<MvxViewPagerFragmentInfo>
                {
					new MvxViewPagerFragmentInfo("RecyclerView 1", typeof(RecyclerViewFragment),
                                                                       typeof(RecyclerViewModel)),
					new MvxViewPagerFragmentInfo("RecyclerView 2", typeof(RecyclerViewFragment),
                                                                       typeof(RecyclerViewModel)),
					new MvxViewPagerFragmentInfo("RecyclerView 3", typeof(RecyclerViewFragment),
                                                                       typeof(RecyclerViewModel)),
					new MvxViewPagerFragmentInfo("RecyclerView 4", typeof(RecyclerViewFragment),
                                                                       typeof(RecyclerViewModel)),
					new MvxViewPagerFragmentInfo("RecyclerView 5", typeof(RecyclerViewFragment),
                                                                       typeof(RecyclerViewModel))
                };
				viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }
    }
}