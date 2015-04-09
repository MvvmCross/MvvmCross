using System;

using AppKit;

namespace Soba.XamMac.Unified
{
	static class MainClass
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();
			NSApplication.Main (args);
		}
	}
}
