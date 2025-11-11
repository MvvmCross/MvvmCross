// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;
using AndroidX.Core.View;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using MvvmCross.Platforms.Android.Views;
using Playground.Core.ViewModels;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme",
        WindowSoftInputMode = SoftInput.AdjustPan)]
    public class RootView : MvxActivity<RootViewModel>, IOnApplyWindowInsetsListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RootView);

            ViewCompat.SetOnApplyWindowInsetsListener(FindViewById(Resource.Id.main_frame), this);
        }

        public WindowInsetsCompat OnApplyWindowInsets(View v, WindowInsetsCompat insets)
        {
            var inset = insets.GetInsets(WindowInsetsCompat.Type.SystemBars());

            (v.LayoutParameters as FrameLayout.LayoutParams).SetMargins(
                inset.Left,
                inset.Top,
                inset.Right,
                inset.Bottom);

            return WindowInsetsCompat.Consumed;
        }
    }
}
