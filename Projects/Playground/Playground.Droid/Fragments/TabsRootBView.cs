﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.DroidX.Fragments;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Fragments
{
    [MvxFragmentPresentation(fragmentHostViewType: typeof(SplitDetailView), fragmentContentId: Resource.Id.tabs_frame, addToBackStack: true)]
    [Register(nameof(TabsRootBView))]
    public class TabsRootBView : MvxFragment<TabsRootBViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.TabsRootBView, null);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            if (savedInstanceState == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }
        }
    }
}
