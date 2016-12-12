using System;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;
using MvvmCross.iOS.Support.Views;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Root)]
	public class MainView : MvxBaseTabBarViewController<MainViewModel>
	{
		private bool isPresentedFirstTime = true;

		public MainView()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			if(this.ViewModel != null && this.isPresentedFirstTime)
			{
				this.isPresentedFirstTime = false;
				this.ViewModel.ShowInitialViewModelsCommand.Execute(null);
			}
		}
	}
}

