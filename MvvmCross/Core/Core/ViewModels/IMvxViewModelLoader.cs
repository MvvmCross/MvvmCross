// IMvxViewModelLoader.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxViewModelLoader
    {
        IMvxViewModel LoadViewModel(MvxViewModelRequest request, IMvxBundle savedState);

        IMvxViewModel ReloadViewModel(IMvxViewModel viewModel, MvxViewModelRequest request, IMvxBundle savedState);
    }
}