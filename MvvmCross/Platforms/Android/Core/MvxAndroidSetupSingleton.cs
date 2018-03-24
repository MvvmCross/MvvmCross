﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Android.Core
{
    public class MvxAndroidSetupSingleton
        : MvxSetupSingleton
    {
        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Context applicationContext)
        {
            var instance = EnsureSingletonAvailable<MvxAndroidSetupSingleton>();
            instance.PlatformSetup<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);
            return instance;
        }
    }
}
