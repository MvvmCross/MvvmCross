// IMvxWinRTViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.WinRT.Interfaces
{
    public interface IMvxWinRTViewPresenter
    {
        void Show(MvxShowViewModelRequest request);
        void Close(IMvxViewModel viewModel);
    }
}