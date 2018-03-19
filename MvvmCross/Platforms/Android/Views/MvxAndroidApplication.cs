// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platform.Android.Core;
using MvvmCross.ViewModels;

namespace MvvmCross.Platform.Android.Views
{
    public class MvxAndroidApplication : Application, IMvxAndroidApplication
    {
        public static MvxAndroidApplication Instance { get; private set; }

        public MvxAndroidApplication()
        {
            Instance = this;
        }

        public MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Instance = this;
        }
    }

    public abstract class MvxAndroidApplication<TMvxAndroidSetup, TApplication> : MvxAndroidApplication
      where TMvxAndroidSetup : MvxAndroidSetup<TApplication>, new()
      where TApplication : IMvxApplication, new()
    {
        static MvxAndroidApplication()
        {
            MvxSetup.RegisterSetupType<TMvxAndroidSetup>();
        }

        public MvxAndroidApplication() : base()
        {
        }

        public MvxAndroidApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
    }
}
