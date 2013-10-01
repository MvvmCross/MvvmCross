using System;
using Android.App;
using Android.Net;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid;

namespace Cirrious.MvvmCross.Plugins.Network.Droid
{
	public class MvxReachability: IMvxReachability 
	{
		private ConnectivityManager _connectivityManager;
		protected ConnectivityManager ConnectivityManager {
			get { return _connectivityManager ?? (_connectivityManager = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.GetSystemService(Application.ConnectivityService) as ConnectivityManager);}
		}

		public bool IsHostReachable(string host)
		{
			var activeNetwork = ConnectivityManager.ActiveNetworkInfo;
			return activeNetwork != null && activeNetwork.IsConnected;
		}
	}
}

