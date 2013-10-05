// MvxReachability.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Android.Content;
using Android.Net;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.Plugins.Network.Reachability;
using Java.Net;

namespace Cirrious.MvvmCross.Plugins.Network.Droid
{
    public class MvxReachability : IMvxReachability
    {
        private const int ReachableTimeoutInMilliseconds = 5000;

        private ConnectivityManager _connectivityManager;

        protected ConnectivityManager ConnectivityManager
        {
            get
            {
                _connectivityManager = _connectivityManager ?? (ConnectivityManager)(Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.GetSystemService(Context.ConnectivityService));
                return _connectivityManager;
            }
        }

        protected bool IsConnected
        {
            get
            {
                try
                {
                    var activeConnection = ConnectivityManager.ActiveNetworkInfo;

                    return ((activeConnection != null) && activeConnection.IsConnected);
                }
                catch (Exception e)
                {
                    Mvx.Warning("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission - error: {0}", e.ToLongString());
                    return false;
                }
            }
        }

        public bool IsHostReachable(string host)
        {
            bool reachable = false;

            if (IsConnected)
            {
                if (!string.IsNullOrEmpty(host))
                {
                    // to avoid ping issues we return true if we have a network here
                    reachable = true;
                }
            }

            return reachable;
        }

        public bool IsHostPingReachable(string host)
        {
            bool reachable = false;

            if (IsConnected)
            {
                if (!string.IsNullOrEmpty(host))
                {
                    try
                    {
                        reachable = InetAddress.GetByName(host).IsReachable(ReachableTimeoutInMilliseconds);
                    }
                    catch (UnknownHostException)
                    {
                        reachable = false;
                    }
                }
            }

            return reachable;
        }
    }
}

/*
protected MvxReachabilityStatus GetReachabilityType()
{
    if (IsConnected)
    {
        var wifiState = ConnectivityManager.GetNetworkInfo(ConnectivityType.Wifi).GetState();
        if (wifiState == NetworkInfo.State.Connected)
        {
            // We are connected via WiFi
            return MvxReachabilityStatus.ViaWiFiNetwork;
        }

        var mobile = ConnectivityManager.GetNetworkInfo(ConnectivityType.Mobile).GetState();
        if (mobile == NetworkInfo.State.Connected)
        {
            // We are connected via carrier data
            return MvxReachabilityStatus.ViaCarrierDataNetwork;
        }
    }

    return MvxReachabilityStatus.Not;
}
*/
