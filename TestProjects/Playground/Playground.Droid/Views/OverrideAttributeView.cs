using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame)]
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [Register(nameof(OverrideAttributeView))]
    public class OverrideAttributeView : MvxFragment<OverrideAttributeViewModel>, IMvxOverridePresentationAttribute
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ChildView, null);

            return view;
        }

        public MvxBasePresentationAttribute PresentationAttribute()
        {
            return new MvxFragmentPresentationAttribute(
                typeof(RootViewModel),
                Resource.Id.content_frame,
                false,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out,
                Resource.Animation.abc_fade_in,
                Resource.Animation.abc_fade_out);
        }
    }
}
