// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content.PM;
using MvvmCross.Platforms.Android.Views;

namespace Playground.Droid
{
    [Activity(
        Label = "Playground.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    [RequiresUnreferencedCode("MvxStartActivity require unreferenced code")]
    public class SplashScreen : MvxStartActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }
    }
}
