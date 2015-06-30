using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Support.AppCompat;
using Cirrious.MvvmCross.Droid.Support.Fragging.Fragments;
using Cirrious.MvvmCross.Droid.Support.RecyclerView;
using Cirrious.MvvmCross.Droid.Support.V4;
using Example.Core.ViewModels;
using Example.Droid.Activities;

namespace Example.Droid.Fragments
{
    [Register("example.droid.fragments.RecyclerViewFragment")]
    public class RecyclerViewFragment : MvxFragment<RecyclerViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.fragment_recyclerview, null);

            MvxRecyclerView recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null) {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager (Activity);
                recyclerView.SetLayoutManager (layoutManager);
            }

            return view;
        }
    }
}

