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
    [MvxFragmentPresentation(fragmentHostViewType: typeof(SecondChildView), fragmentContentId: Resource.Id.nested_frame)]
    [Register(nameof(NestedChildView))]
    public class NestedChildView : MvxFragment<NestedChildViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.NestedChildView, null);

            return view;
        }
    }
}
