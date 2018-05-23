// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
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

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
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
