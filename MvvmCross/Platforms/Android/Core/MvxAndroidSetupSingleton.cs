// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Android.Core
{
    public class MvxAndroidSetupSingleton
        : MvxSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Application applicationContext)
        {
            var instance = EnsureSingletonAvailable<MvxAndroidSetupSingleton>();
            instance.PlatformSetup<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);
            return instance;
        }
    }
}
