using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.iOS.Support.Tabs.Core;

namespace MvvmCross.iOS.Support.Tabs.iOS
{
	public class Setup : MvxIosSetup
	{
		public Setup(MvxApplicationDelegate delg, MvxBaseIosViewPresenter presenter) : base(delg, presenter)
		{

		}

		protected override IMvxApplication CreateApp() => new App();
	}
}

