// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Playground.Droid
{
    [Activity(
        Label = "Playground.Droid"
        , MainLauncher = true
        , Icon = "@mipmap/icon"
        , Theme = "@style/AppTheme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    [Register("playground.SplashScreen")]
    public class SplashScreen : AppCompatActivity, IStartupActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Note: this could be moved into AppTheme.Splash
            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.SplashScreen);
        }

        public void FinishActivity()
        {
            Finish();
        }
    }
}
