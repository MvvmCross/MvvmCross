using System;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross
{
	public interface IMvxPagedViewModel : IMvxViewModel
	{
		string PagedViewId { get; }
	}

	public interface IMvxPageViewModel : IMvxViewModel
	{
		IMvxPagedViewModel GetDefaultViewModel ();
		IMvxPagedViewModel GetNextViewModel(IMvxPagedViewModel currentViewModel);
		IMvxPagedViewModel GetPreviousViewModel(IMvxPagedViewModel currentViewModel);
	}
}
