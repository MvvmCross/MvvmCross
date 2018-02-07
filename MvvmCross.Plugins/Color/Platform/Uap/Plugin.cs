// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform.UI;

namespace MvvmCross.Plugin.Color.Platform.Uap
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMvxNativeColor>(new MvxWindowsCommonColor());
        }
    }
}
