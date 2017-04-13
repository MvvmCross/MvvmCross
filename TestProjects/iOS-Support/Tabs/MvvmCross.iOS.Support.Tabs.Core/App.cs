using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.Tabs.Core
{
	public class App : MvxApplication
	{
		public App()
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			this.RegisterAppStart(new AppStart());
		}
	}
}

