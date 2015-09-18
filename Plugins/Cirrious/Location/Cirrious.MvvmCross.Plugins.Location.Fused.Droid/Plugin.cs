using System;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Plugins.Location.Droid
{
	public class Plugin
		: IMvxPlugin
	{
		public void Load()
		{
			Mvx.RegisterSingleton<IMvxLocationWatcher>(() => new MvxAndroidFusedLocationWatcher());
		}
	}
}

