// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Content;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Android.Core
{
    public class MvxAndroidSetupSingleton
        : MvxSetupSingleton
    {
        public virtual void EnsureInitialized(Context applicationContext)
        {
            MvxSetup.PlatformInstance<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);
            base.EnsureInitialized();
        }
        public virtual void InitializeFromSplashScreen(IMvxSetupMonitor splashScreen, Context applicationContext)
        {
            MvxSetup.PlatformInstance<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);
            base.InitializeFromSplashScreen(splashScreen);
        }
    }
}
