using System;
using Cirrious.MvvmCross.ViewModels;

namespace MvxPageDemo.ViewModels
{
	public class FirstPageViewModel : MvxViewModel, IMvxPagedViewModel
	{
		public string PagedViewId { get { return("FirstPage"); } }
		private string _pageTitle = null;
		public string PageTitle
		{
			get { return(_pageTitle); }
			set { _pageTitle = value; RaisePropertyChanged (() => PageTitle); }
		}

		public FirstPageViewModel ()
		{
			_pageTitle = "First Page";
		}
	}
}
