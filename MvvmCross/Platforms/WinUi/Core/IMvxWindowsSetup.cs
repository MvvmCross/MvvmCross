// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Core;
using MvvmCross.Platforms.WinUi.Views;
using Windows.ApplicationModel.Activation;
using Microsoft.UI.Xaml.Controls;

namespace MvvmCross.Platforms.WinUi.Core
{
    public interface IMvxWindowsSetup : IMvxSetup
    {
        void PlatformInitialize(Frame rootFrame, string activatedEventArgs, string suspensionManagerSessionStateKey = null);
        void PlatformInitialize(Frame rootFrame, string suspensionManagerSessionStateKey = null);
        void PlatformInitialize(IMvxWindowsFrame rootFrame);
        void UpdateActivationArguments(string e);
    }
}
