using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using Cirrious.CrossCore.Exceptions;

namespace TwitterSearch.UI.Mac
{
	class MainClass
	{
		static void Main (string[] args)
		{
			var t = new []
			{
				typeof(Cirrious.MvvmCross.Platform.MvxSetup),
			    typeof(Core.TwitterSearchApp),
				typeof(Cirrious.CrossCore.Plugins.MvxLoaderPluginRegistry)
			};
			try {

				NSApplication.Init ();
				NSApplication.Main (args);
			} catch (Exception e) {
				var s = e.ToLongString();
				var k = ""; 
			}
		}
	}
}	

