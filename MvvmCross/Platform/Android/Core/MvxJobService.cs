// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.App.Job;
using Android.Runtime;

namespace MvvmCross.Platform.Android.Core
{
    [Register("mvvmcross.platform.android.core.MvxJobService")]
    public abstract class MvxJobService : JobService
    {
        protected MvxJobService(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected MvxJobService()
        {
        }

        public override bool OnStartJob(JobParameters @params)
        {
            var setup = MvxAndroidSetupSingleton.EnsureSingletonAvailable(this);
            setup.EnsureInitialized();
            return true;
        }
    }
}
