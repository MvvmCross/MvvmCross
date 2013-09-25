using Cirrious.MvvmCross.ViewModels;

namespace DevDemo.Core.ViewModels
{
	public class DevViewModel 
		: MvxViewModel
	{
		private string _hello = "Hello MvvmCross";
		public string Hello
		{ 
			get { return _hello; }
			set { _hello = value; RaisePropertyChanged(() => Hello); }
		}

		private string _lorem = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
		public string Lorem
		{
			get { return _lorem; }
			set { _lorem = value;
				RaisePropertyChanged (() => Lorem); }
		}
	}
}

