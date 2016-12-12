using System;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.iOS.Support.ExpandableTableView.Core
{
	public class App
		: MvxApplication
	{
		public override void Initialize()
		{
			RegisterAppStart<FirstViewModel>();
		}
	}
}