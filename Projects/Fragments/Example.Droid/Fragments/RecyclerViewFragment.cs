﻿using System;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using AndroidX.AppCompat.Widget;
using Android.Views;
using Android.Widget;
using Example.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platforms.Android.Views.Fragments;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform.WeakSubscription;

namespace Example.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(MainViewModel), Resource.Id.content_frame, true)]
    [Register("example.droid.fragments.RecyclerViewFragment")]
    public class RecyclerViewFragment : MvxFragment<RecyclerViewModel>
    {
        private IDisposable _itemSelectedToken;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_recyclerview, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);
                (recyclerView.Adapter as IMvxRecyclerAdapterBindableHolder).MvxViewHolderBound += (args) =>
                {
                    var item = args.Holder.ItemView;
                    // you can access bound holder item here.
                };
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
    }
}