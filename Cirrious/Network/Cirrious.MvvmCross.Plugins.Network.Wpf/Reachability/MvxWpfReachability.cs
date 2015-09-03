using System.Net.NetworkInformation;
using Cirrious.MvvmCross.Plugins.Network.Reachability;

namespace Cirrious.MvvmCross.Plugins.Network.Wpf.Reachability
{
    public class MvxWpfReachability : IMvxReachability
    {
        public bool IsHostReachable(string host)
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}