// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;
using MvvmCross.Forms.Presenters;
using Playground.Core.ViewModels;
using Playground.Forms.UI;
using Xamarin.Forms;

namespace Playground.Forms.Droid
{
    [Activity(
        Label = "Playground.Forms",
        Icon = "@mipmap/icon",
        Theme = "@style/AppTheme",
        // MainLauncher = true, // No Splash Screen: Uncomment this lines if removing splash screen
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        LaunchMode = LaunchMode.SingleTask)]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
    // No Splash Screen: use this base instead
    // MvxFormsAppCompatActivity<Setup, Core.App, FormsApp, MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
