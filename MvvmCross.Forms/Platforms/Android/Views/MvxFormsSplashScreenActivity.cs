// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Forms.Platforms.Android.Views
{
    public abstract class MvxFormsSplashScreenActivity<TMvxAndroidSetup, TApplication, TFormsApplication> : MvxSplashScreenActivity
            where TMvxAndroidSetup : MvxFormsAndroidSetup<TApplication, TFormsApplication>, new()
            where TApplication : class, IMvxApplication, new()
            where TFormsApplication : Application, new()
    {
        protected MvxFormsSplashScreenActivity(int resourceId = NoContent) : base(resourceId)
        {
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
