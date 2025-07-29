// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Core;

namespace MvvmCross.Platforms.Ios.Core
{
    public class MvxIosSetupSingleton
        : MvxSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public static MvxIosSetupSingleton EnsureSingletonAvailable(IMvxLifetime lifetimeInstance, UIWindow window)
        {
            var instance = EnsureSingletonAvailable<MvxIosSetupSingleton>();
            instance.PlatformSetup<MvxIosSetup>()?.PlatformInitialize(lifetimeInstance, window);
            return instance;
        }
    }
}
