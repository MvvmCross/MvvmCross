using System;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace MvvmCross.Plugins.Location.Fused.Droid
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

