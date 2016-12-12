using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
	public class Tab1ViewModel : MvxViewModel
	{
		public Tab1ViewModel()
		{
			OpenChildCommand = new MvxCommand(() => ShowViewModel<TabChildViewModel>());
			OpenModalCommand = new MvxCommand(() => ShowViewModel<ModalViewModel>());
		}

		public ICommand OpenChildCommand { get; private set; }

		public ICommand OpenModalCommand { get; private set; }

	}
}

