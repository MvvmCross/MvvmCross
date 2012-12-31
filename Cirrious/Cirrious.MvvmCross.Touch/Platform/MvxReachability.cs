#region Copyright

// <copyright file="MvxReachability.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Net;
using Cirrious.MvvmCross.Interfaces.Platform;
using MonoTouch.CoreFoundation;
using MonoTouch.SystemConfiguration;

namespace Cirrious.MvvmCross.Touch.Platform
{
    public class MvxReachability
        : IMvxReachability
    {
        private const string DefaultHostName = "www.google.com";

        // Is the host reachable with the current network configuration
        public bool IsHostReachable(string host)
        {
            return StaticIsHostReachable(host);
        }

        public static bool StaticIsHostReachable(string host)
        {
            if (host == null || host.Length == 0)
                return false;

            using (var r = new NetworkReachability(host))
            {
                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }

        public static bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        // 
        // Raised every time there is an interesting reachable event, 
        // we do not even pass the info as to what changed, and 
        // we lump all three status we probe into one
        //
        public static event EventHandler ReachabilityChanged;

        private static void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }

        //
        // Returns true if it is possible to reach the AdHoc WiFi network
        // and optionally provides extra network reachability flags as the
        // out parameter
        //
        private static NetworkReachability adHocWiFiNetworkReachability;

        public static bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] {169, 254, 0, 0}));
#warning Need to look at SetNotification instead - ios6 change
                adHocWiFiNetworkReachability.SetCallback(OnChange);
                adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            if (!adHocWiFiNetworkReachability.TryGetFlags(out flags))
                return false;

            return IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability defaultRouteReachability;

        private static bool IsNetworkAvaialable(out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability(new IPAddress(0));
#warning Need to look at SetNotification instead - ios6 change
                defaultRouteReachability.SetCallback(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if (defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability remoteHostReachability;

        public static MvxNetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (remoteHostReachability == null)
            {
                remoteHostReachability = new NetworkReachability(DefaultHostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = remoteHostReachability.TryGetFlags(out flags);

#warning Need to look at SetNotification instead - ios6 change
                remoteHostReachability.SetCallback(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
                reachable = remoteHostReachability.TryGetFlags(out flags);

            if (!reachable)
                return MvxNetworkStatus.NotReachable;

            if (!IsReachableWithoutRequiringConnection(flags))
                return MvxNetworkStatus.NotReachable;

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return MvxNetworkStatus.ReachableViaCarrierDataNetwork;

            return MvxNetworkStatus.ReachableViaWiFiNetwork;
        }

        public static MvxNetworkStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvaialable(out flags);
            if (defaultNetworkAvailable)
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return MvxNetworkStatus.NotReachable;
            }
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return MvxNetworkStatus.ReachableViaCarrierDataNetwork;
            return MvxNetworkStatus.ReachableViaWiFiNetwork;
        }

        public static MvxNetworkStatus LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return MvxNetworkStatus.ReachableViaWiFiNetwork;
            }
            return MvxNetworkStatus.NotReachable;
        }
    }
}