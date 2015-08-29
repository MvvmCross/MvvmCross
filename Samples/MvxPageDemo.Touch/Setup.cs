using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Touch.Platform;
using UIKit;
using MvxPageDemo.Shared;
using Cirrious.MvvmCross.Touch.Views.Presenters;

namespace MvxPageDemo.Touch
{
	public class Setup : MvxTouchSetup
	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return(new App());
		}

		protected override IMvxTouchViewPresenter CreatePresenter ()
		{
			return(new MvxModalNavSupportTouchViewPresenter ((MvxApplicationDelegate)ApplicationDelegate, Window));
		}
	}
}