// IMvxIosViewCreator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using MvvmCross.Core.ViewModels;

    public interface IMvxIosViewCreator : IMvxCurrentRequest
    {
        IMvxIosView CreateView(MvxViewModelRequest request);

        IMvxIosView CreateView(IMvxViewModel viewModel);
    }
}