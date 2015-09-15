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
    [Register("example.droid.fragments.ExamplesFragment")]
    public class ExamplesFragment : BaseFragment<ExamplesViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
            if (viewPager != null)
            {
                var fragments = new List<MvxFragmentStatePagerAdapter.FragmentInfo>
                {
                    new MvxFragmentStatePagerAdapter.FragmentInfo("RecyclerView", typeof(RecyclerViewFragment), typeof(RecyclerViewModel))
                };
                viewPager.Adapter = new MvxFragmentStatePagerAdapter(Activity, ChildFragmentManager, fragments);
            }

            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewPager);

            return view;
        }

        protected override int FragmentId {
            get {
                return Resource.Layout.fragment_examples;
            }
        }
    }
}