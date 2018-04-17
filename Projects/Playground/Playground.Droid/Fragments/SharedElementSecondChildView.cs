// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Transitions;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(SharedElementRootViewModel), Resource.Id.shared_content_frame, true)]
    public class SharedElementSecondChildView : MvxFragment<SharedElementSecondChildViewModel>
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                SharedElementEnterTransition = TransitionInflater.From(Activity).InflateTransition(Android.Resource.Transition.Move);
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SharedElementSecondChildView, null);
            Arguments.SetSharedElementsById(view);

            return view;
        }
    }
}
