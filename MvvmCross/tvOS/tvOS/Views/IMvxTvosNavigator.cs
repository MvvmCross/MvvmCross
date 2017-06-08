// IMvxTvosNavigator.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
    public interface IMvxTvosNavigator
    {
        void NavigateTo(MvxViewModelRequest request);

        void Close(IMvxViewModel toClose);
    }
}