// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Views
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    public class RootView : MvxAppCompatActivity<RootViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RootView);
        }
    }
}
