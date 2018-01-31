// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Runtime;

namespace MvvmCross.Droid.Views
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
}
