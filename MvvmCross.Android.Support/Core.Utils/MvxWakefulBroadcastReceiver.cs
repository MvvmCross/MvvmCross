// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.Content;
using MvvmCross.Droid.Platform;

namespace MvvmCross.Droid.Support.V4
{
    [Register("mvvmcross.droid.support.v4.MvxBrowseSupportFragment")]
    public abstract class MvxWakefulBroadcastReceiver : WakefulBroadcastReceiver
    {
        protected MvxWakefulBroadcastReceiver(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxWakefulBroadcastReceiver()
        {
        }

        public async override void OnReceive(Context context, Intent intent)
        {
            var setup = await MvxAndroidSetupSingleton.EnsureSingletonAvailable(context);
            await setup.EnsureInitialized();
        }
    }
}
