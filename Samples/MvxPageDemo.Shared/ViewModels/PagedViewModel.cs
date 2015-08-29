using System;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross;

namespace MvxPageDemo.ViewModels
{
	public class PagedViewModel : MvxViewModel, IMvxPageViewModel
	{
		private FirstPageViewModel _firstPageVM = null;
		private SecondPageViewModel _secondPageVM = null;
		private ThirdPageViewModel _thirdPageVM = null;

		public PagedViewModel ()
		{
			_firstPageVM = new FirstPageViewModel ();
			_secondPageVM = new SecondPageViewModel ();
			_thirdPageVM = new ThirdPageViewModel ();
		}

		public IMvxPagedViewModel GetDefaultViewModel ()
		{
			return(_firstPageVM);
		}

		public IMvxPagedViewModel GetNextViewModel (IMvxPagedViewModel currentViewModel)
		{
			if (currentViewModel is FirstPageViewModel)
				return(_secondPageVM);
			else if (currentViewModel is SecondPageViewModel)
				return(_thirdPageVM);
			return(null);
		}

		public IMvxPagedViewModel GetPreviousViewModel (IMvxPagedViewModel currentViewModel)
		{
			if (currentViewModel is ThirdPageViewModel)
				return(_secondPageVM);
			else if (currentViewModel is SecondPageViewModel)
				return(_firstPageVM);
			return(null);
		}
	}
}
