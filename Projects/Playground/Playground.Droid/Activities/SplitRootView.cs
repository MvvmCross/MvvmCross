// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Content.PM;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;
using Playground.Droid.Extensions;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    [RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
    public class SplitRootView : MvxActivity<SplitRootViewModel>
    {
        public DrawerLayout DrawerLayout { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SplitRootView);

            DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            if (savedInstanceState == null)
            {
                ViewModel.ShowInitialMenuCommand.Execute();
                ViewModel.ShowDetailCommand.Execute();
            }

            OnBackPressedDispatcher.AddCallback(this, new BackPressedCallback(true, BackPressed));
        }

        private void BackPressed()
        {
            if (DrawerLayout?.IsDrawerOpen(GravityCompat.Start) is true)
                DrawerLayout.CloseDrawers();
            else
                Finish();
        }
    }
}
