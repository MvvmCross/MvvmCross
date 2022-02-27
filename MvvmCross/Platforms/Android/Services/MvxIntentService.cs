// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using MvvmCross.Core;
using MvvmCross.Platforms.Android.Core;

namespace MvvmCross.Platforms.Android.Services
{
    [Register("mvvmcross.droid.services.MvxIntentService")]
    public abstract class MvxIntentService : IntentService
    {
        protected MvxIntentService(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected MvxIntentService(string name) : base(name)
        {
        }

        protected override void OnHandleIntent(Intent intent)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
            setup.EnsureInitialized();
        }
    }
}
