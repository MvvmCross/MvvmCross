// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using MvvmCross.Plugins.Network.Rest;

namespace MvvmCross.Plugins.Network.Uwp
{
    public class Plugin
        : IMvxPlugin
    {
        public void Load()
        {
#warning TODO - WINDOWS STORE SHOULD ADD IMvxReachability!
            // Mvx.RegisterType<IMvxReachability, MvxReachability>();
            Mvx.RegisterType<IMvxRestClient, MvxJsonRestClient>();
            Mvx.RegisterType<IMvxJsonRestClient, MvxJsonRestClient>();
        }
    }
}