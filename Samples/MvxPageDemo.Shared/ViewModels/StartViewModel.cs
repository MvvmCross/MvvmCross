using System;
using Cirrious.MvvmCross.ViewModels;

namespace MvxPageDemo.ViewModels
{
	public class StartViewModel : MvxViewModel
	{
		private MvxCommand _showCommand = null;
		public IMvxCommand ShowCommand
		{
			get {
				_showCommand = _showCommand ?? new MvxCommand (DoShowCommand);
				return(_showCommand);
			}
		}

		public StartViewModel ()
		{
		}

		private void DoShowCommand()
		{
			ShowViewModel<PagedViewModel> ();
		}
	}
}
