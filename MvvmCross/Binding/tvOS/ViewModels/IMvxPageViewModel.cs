// IMvxPageViewModel.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.tvOS.ViewModels
{
	using MvvmCross.Core.ViewModels;

	public interface IMvxPageViewModel : IMvxViewModel
	{
		int PageIndex { get; }
	}
}
