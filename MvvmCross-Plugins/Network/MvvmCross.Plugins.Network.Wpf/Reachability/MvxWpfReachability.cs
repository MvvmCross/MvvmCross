using System.Net.NetworkInformation;
using MvvmCross.Plugins.Network.Reachability;

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