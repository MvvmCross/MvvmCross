// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.OS;
using Android.Runtime;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform.Android.Binding.BindingContext;
using MvvmCross.Platform.Android.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Playground.Core.ViewModels;

namespace Playground.Forms.Droid.Views
{
    [Register(nameof(OverrideAttributeView))]
    [MvxFragmentPresentation(AddToBackStack = true)]
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
            return new MvxFragmentPresentationAttribute() { AddToBackStack = true };
        }
    }
}
