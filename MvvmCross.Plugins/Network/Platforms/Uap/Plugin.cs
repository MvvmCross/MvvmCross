// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Plugin.Network.Platforms.Uap
{
    [MvxPlugin]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
#warning TODO - WINDOWS STORE SHOULD ADD IMvxReachability!
            // Mvx.IoCProvider.RegisterType<IMvxReachability, MvxReachability>();
        }
    }
}
