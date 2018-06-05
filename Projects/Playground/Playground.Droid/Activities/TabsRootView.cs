// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Playground.Core.ViewModels;

namespace Playground.Droid.Activities
{
    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class TabsRootView : MvxAppCompatActivity<TabsRootViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.TabsRootView);

            if (bundle == null)
            {
                ViewModel.ShowInitialViewModelsCommand.Execute();
            }
        }
    }
}
