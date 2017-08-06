using System;
using Android;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragment(typeof(SplitRootViewModel), FragmentContentId = Resource.Id.split_content_frame)]
    [Register(nameof(SplitDetailView))]
    public class SplitDetailView : MvxFragment<SplitDetailViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SplitDetailView, null);

            return view;
        }
    }
}
