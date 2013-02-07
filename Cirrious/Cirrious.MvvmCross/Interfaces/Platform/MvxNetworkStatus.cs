// MvxNetworkStatus.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace Cirrious.MvvmCross.Interfaces.Platform
{
#warning Move to Plugin (or lose!)
    public enum MvxNetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }
}