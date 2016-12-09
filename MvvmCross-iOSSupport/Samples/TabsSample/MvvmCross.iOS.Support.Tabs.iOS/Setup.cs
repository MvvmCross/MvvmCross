using System;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.iOS.Support.Tabs.Core;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;

namespace MvvmCross.iOS.Support.Tabs.iOS
{
	public class Setup : MvxIosSetup
	{
		public Setup(MvxApplicationDelegate delg, MvxBaseIosViewPresenter presenter) : base(delg, presenter)
		{

		}

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            var presenter = new MvxTabsViewPresenter((MvxApplicationDelegate)ApplicationDelegate, Window);
            Mvx.RegisterSingleton<IMvxTabBarPresenter>(presenter);
            return presenter;
        }

		protected override IMvxApplication CreateApp() => new App();
	}
}

