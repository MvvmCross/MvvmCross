using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross;

namespace MvxPageDemo.ViewModels
{
	public class SecondPageViewModel : MvxViewModel, IMvxPagedViewModel
	{
		public string PagedViewId { get { return("SecondPage"); } }
		private string _pageTitle = null;
		public string PageTitle
		{
			get { return(_pageTitle); }
			set { _pageTitle = value; RaisePropertyChanged (() => PageTitle); }
		}

		public SecondPageViewModel ()
		{
			_pageTitle = "Second Page";
		}
	}
}
