// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using MvvmCross.Core;
using MvvmCross.Platforms.Mac.Presenters;

namespace MvvmCross.Platforms.Mac.Core
{
    public class MvxMacSetupSingleton
        : MvxSetupSingleton
    {
        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate)
        {
            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate);
            return instance;
        }

        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate, presenter);
            return instance;
        }
    }
}
