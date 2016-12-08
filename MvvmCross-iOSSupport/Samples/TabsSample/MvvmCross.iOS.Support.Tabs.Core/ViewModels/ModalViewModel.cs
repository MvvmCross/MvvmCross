using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
namespace MvvmCross.iOS.Support.Tabs.Core.ViewModels
{
	public class ModalViewModel : MvxViewModel
	{
		public ModalViewModel()
		{
			CloseCommand = new MvxCommand(() => Close(this));
		}

		public ICommand CloseCommand { get; private set; }
	}
}

