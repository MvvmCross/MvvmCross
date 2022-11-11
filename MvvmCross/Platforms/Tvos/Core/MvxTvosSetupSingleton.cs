// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Platforms.Tvos.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Core
{
    public class MvxTvosSetupSingleton
        : MvxSetupSingleton
    {
        public static MvxTvosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            var instance = EnsureSingletonAvailable<MvxTvosSetupSingleton>();
            instance.PlatformSetup<MvxTvosSetup>()?.PlatformInitialize(applicationDelegate, window);
            return instance;
        }

        public static MvxTvosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, IMvxTvosViewPresenter presenter)
        {
            var instance = EnsureSingletonAvailable<MvxTvosSetupSingleton>();
            instance.PlatformSetup<MvxTvosSetup>()?.PlatformInitialize(applicationDelegate, presenter);
            return instance;
        }
    }
}
