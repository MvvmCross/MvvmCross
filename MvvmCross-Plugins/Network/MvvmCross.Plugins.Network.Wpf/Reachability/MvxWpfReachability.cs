using MvvmCross.Plugins.Network.Reachability;
using System.Net.NetworkInformation;

namespace MvvmCross.Plugins.Network.Wpf.Reachability
{
    public class MvxWpfReachability : IMvxReachability
    {
        public bool IsHostReachable(string host)
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}