using System;
using MvvmCross.iOS.Support.Presenters;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Support.Tabs.Core.ViewModels;

namespace MvvmCross.iOS.Support.Tabs.iOS.Views
{
	[MvxTabPresentation(MvxTabPresentationMode.Child)]
	public class TabChildView : MvxViewController<TabChildViewModel>
	{
		public TabChildView()
		{
		}
	}
}

