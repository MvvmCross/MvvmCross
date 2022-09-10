// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Platforms.Ios.Presenters;
using UIKit;

namespace MvvmCross.Platforms.Ios.Core
{
    public interface IMvxIosSetup : IMvxSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, UIWindow window);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter);
    }
}
