// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin.Network.Platforms.Wpf.Reachability;
using MvvmCross.Plugin.Network.Reachability;

namespace MvvmCross.Plugin.Network.Platforms.Wpf
{
    [MvxPlugin]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            Mvx.IoCProvider.RegisterType<IMvxReachability, MvxWpfReachability>();
        }
    }
}
