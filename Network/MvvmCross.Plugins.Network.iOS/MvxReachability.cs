// MvxReachability.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Net;
using SystemConfiguration;
using CoreFoundation;
using MvvmCross.Plugins.Network.Reachability;

namespace MvvmCross.Plugins.Network.iOS
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
            if (string.IsNullOrEmpty(host))
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
            h?.Invoke(null, EventArgs.Empty);
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
                adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
#warning Need to look at SetNotification instead - ios6 change
                adHocWiFiNetworkReachability.SetNotification(OnChange);
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
                defaultRouteReachability.SetNotification(OnChange);
                defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if (defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        private static NetworkReachability remoteHostReachability;

        public static MvxReachabilityStatus RemoteHostStatus()
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
                remoteHostReachability.SetNotification(OnChange);
                remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
                reachable = remoteHostReachability.TryGetFlags(out flags);

            if (!reachable)
                return MvxReachabilityStatus.Not;

            if (!IsReachableWithoutRequiringConnection(flags))
                return MvxReachabilityStatus.Not;

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return MvxReachabilityStatus.ViaCarrierDataNetwork;

            return MvxReachabilityStatus.ViaWiFiNetwork;
        }

        public static MvxReachabilityStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvaialable(out flags);
            if (defaultNetworkAvailable)
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return MvxReachabilityStatus.Not;
            }
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return MvxReachabilityStatus.ViaCarrierDataNetwork;
            return MvxReachabilityStatus.ViaWiFiNetwork;
        }

        public static MvxReachabilityStatus LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return MvxReachabilityStatus.ViaWiFiNetwork;
            }
            return MvxReachabilityStatus.Not;
        }
    }
}