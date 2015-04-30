using System;
using Cirrious.MvvmCross.ViewModels;

namespace MvxPageDemo.ViewModels
{
	public class ThirdPageViewModel : MvxViewModel, IMvxPagedViewModel
	{
		public string PagedViewId { get { return("ThirdPage"); } }
		private string _pageTitle = null;
		public string PageTitle
		{
			get { return(_pageTitle); }
			set { _pageTitle = value; RaisePropertyChanged (() => PageTitle); }
		}

		public ThirdPageViewModel ()
		{
			_pageTitle = "Third Page";
		}
	}
}
