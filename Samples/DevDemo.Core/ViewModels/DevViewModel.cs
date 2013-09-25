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
	}
}

