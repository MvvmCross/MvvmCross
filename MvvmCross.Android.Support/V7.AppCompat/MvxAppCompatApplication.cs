// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxAppCompatApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
  where TMvxAndroidSetup : MvxAppCompatSetup<TApplication>, new()
  where TApplication : class, IMvxApplication, new()
    {
        public MvxAppCompatApplication() : base()
        {
        }

        public MvxAppCompatApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<TMvxAndroidSetup>();
        }
    }
}
