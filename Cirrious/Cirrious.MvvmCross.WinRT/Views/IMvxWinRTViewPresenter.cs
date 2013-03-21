// IMvxWinRTViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.WinRT.Views
{
    public interface IMvxWinRTViewPresenter
    {
        void Show(MvxViewModelRequest request);
        void Close(IMvxViewModel viewModel);
    }
}