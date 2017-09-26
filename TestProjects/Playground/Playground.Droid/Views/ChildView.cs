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
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true, Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out,
                IsCacheableFragment = true)]
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [MvxFragmentPresentation(typeof(TabsRootViewModel), Resource.Id.content_frame)]
    [MvxFragmentPresentation(fragmentHostViewType: typeof(ModalNavView), fragmentContentId: Resource.Id.dialog_content_frame)]
    [Register(nameof(ChildView))]
    public class ChildView : MvxFragment<ChildViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ChildView, null);

            return view;
        }
    }
}
