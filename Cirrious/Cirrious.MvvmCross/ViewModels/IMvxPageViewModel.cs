using System;

namespace Cirrious.MvvmCross.ViewModels
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
