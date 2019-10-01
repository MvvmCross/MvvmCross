// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;
using Playground.Droid.Activities;
using Playground.Droid.Adapter;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(SharedElementRootViewModel), Resource.Id.shared_content_frame)]
    public class SharedElementRootChildView : MvxFragment<SharedElementRootChildViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SharedElementRootChildView, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);

                var adapter = new SelectedItemRecyclerAdapter(BindingContext as IMvxAndroidBindingContext);
                adapter.OnItemClick += AdapterOnItemClick;
                recyclerView.Adapter = adapter;
            }

            return view;
        }

        private void AdapterOnItemClick(object sender, SelectedItemRecyclerAdapter.SelectedItemEventArgs e)
        {
            Toast.MakeText(Activity, $"Selected item {e.Position + 1}", ToastLength.Short)
                .Show();

            (Activity as SharedElementRootView).SelectedListItem = e.Position;

            ViewModel.SelectItemExecution(e.DataContext as ListItemViewModel);
        }
    }
}
