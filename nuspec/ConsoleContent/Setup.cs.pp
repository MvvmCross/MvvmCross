using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Console.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace $rootnamespace$
{
	public class Setup : MvxConsoleSetup
	{
		protected override IMvxApplication CreateApp ()
		{
			return new Core.App();
		}
		
		protected override IMvxTrace CreateDebugTrace()
		{
				return new DebugTrace();
		}
	}
}