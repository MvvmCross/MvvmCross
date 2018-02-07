// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Net.NetworkInformation;
using MvvmCross.Plugins.Network.Reachability;

namespace MvvmCross.Plugin.Network.Platform.Wpf.Reachability
{
    public class MvxWpfReachability : IMvxReachability
    {
        public bool IsHostReachable(string host)
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
