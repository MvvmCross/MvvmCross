// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Plugin.Network.Rest;

namespace MvvmCross.Plugin.Network
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            Mvx.IoCProvider.RegisterType<IMvxRestClient, MvxJsonRestClient>();
            Mvx.IoCProvider.RegisterType<IMvxJsonRestClient, MvxJsonRestClient>();
        }        
    }
}
