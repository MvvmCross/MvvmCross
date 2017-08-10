using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragment(typeof(RootViewModel), FragmentContentId = Resource.Id.content_frame)]
    [MvxFragment(typeof(SplitRootViewModel), FragmentContentId = Resource.Id.split_content_frame)]
    [Register(nameof(SecondChildView))]
    public class SecondChildView : MvxFragment<SecondChildViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SecondChildView, null);

            return view;
        }
    }
}
