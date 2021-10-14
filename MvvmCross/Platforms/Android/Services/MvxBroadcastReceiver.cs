// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using MvvmCross.Platforms.Android.Core;

namespace MvvmCross.Platforms.Android.Services
{
    [Register("mvvmcross.droid.services.MvxBroadcastReceiver")]
    public abstract class MvxBroadcastReceiver : BroadcastReceiver
    {
        protected MvxBroadcastReceiver(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxBroadcastReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(context.ApplicationContext);
            setup.EnsureInitialized();
        }
    }
}
