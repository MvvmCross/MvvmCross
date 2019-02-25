// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using Tizen.Applications;

namespace MvvmCross.Platforms.Tizen.Core
{
    public class MvxTizenSetupSingleton
        : MvxSetupSingleton
    {
        public static MvxTizenSetupSingleton EnsureSingletonAvailable(CoreApplication coreApplication)
        {
            var instance = EnsureSingletonAvailable<MvxTizenSetupSingleton>();
            instance.PlatformSetup<MvxTizenSetup>()?.PlatformInitialize(coreApplication);
            return instance;
        }
    }
}
