// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Runtime;
using MvvmCross.Droid.Support.V7.AppCompat;
using Playground.Core;
using Plugin.CurrentActivity;

namespace Playground.Droid
{
    [Application]
    public class MainApplication : MvxAppCompatApplication<Setup, App>
    {
        [Preserve]
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        [Preserve]
        public MainApplication()
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);
        }
    }
}
