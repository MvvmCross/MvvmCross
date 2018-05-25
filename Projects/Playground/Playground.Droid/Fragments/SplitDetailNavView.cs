// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Runtime;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame, AddToBackStack = true)]
    [Register(nameof(SplitDetailNavView))]
    public class SplitDetailNavView : BaseSplitDetailView<SplitDetailNavViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.SplitDetailNavView;
    }
}
