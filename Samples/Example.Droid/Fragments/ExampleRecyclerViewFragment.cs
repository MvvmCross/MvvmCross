using System;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore.WeakSubscription;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Support.V4;
using Example.Core.ViewModels;
using MvvmCross.Droid.Support.V7.Fragging.Attributes;

namespace Example.Droid.Fragments
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    [Register("example.droid.fragments.ExampleRecyclerViewFragment")]
    public class ExampleRecyclerViewFragment : BaseFragment<ExampleRecyclerViewModel>
    {
        private IDisposable _itemSelectedToken;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
            }

            _itemSelectedToken = ViewModel.WeakSubscribe(() => ViewModel.SelectedItem,
                (sender, args) => {
                    if (ViewModel.SelectedItem != null)
                        Toast.MakeText(Activity,
                            $"Selected: {ViewModel.SelectedItem.Title}",
                            ToastLength.Short).Show();
                });

            var swipeToRefresh = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            var appBar = Activity.FindViewById<AppBarLayout>(Resource.Id.appbar);
            if (appBar != null)
                appBar.OffsetChanged += (sender, args) => swipeToRefresh.Enabled = args.VerticalOffset == 0;

            return view;
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _itemSelectedToken.Dispose();
            _itemSelectedToken = null;
        }

        protected override int FragmentId => Resource.Layout.fragment_example_recyclerview;
    }
}