// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platform.Android.Views;
using MvvmCross.ViewModels;

namespace MvvmCross.Droid.Support.V7.AppCompat
{
    public abstract class MvxAppCompatApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
  where TMvxAndroidSetup : MvxAppCompatSetup<TApplication>, new()
  where TApplication : IMvxApplication, new()
    {
        static MvxAppCompatApplication()
        {
            MvxSetup.RegisterSetupType<TMvxAndroidSetup>();
        }

        public MvxAppCompatApplication() : base()
        {
        }

        public MvxAppCompatApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }

}
