// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using AppKit;
using MvvmCross.Core;
using MvvmCross.Platforms.Mac.Presenters;

namespace MvvmCross.Platforms.Mac.Core
{
    public interface IMvxMacSetup : IMvxSetup
    {
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate);
        void PlatformInitialize(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter);
    }
}
