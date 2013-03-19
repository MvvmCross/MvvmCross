// IMvxTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Touch.Interfaces;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.ViewModels;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchViewPresenter : IMvxTouchModalHost
    {
        void Show(MvxShowViewModelRequest view);
        void Close(IMvxViewModel viewModel);
        void CloseModalViewController();
        void ClearBackStack();
        void RequestRemoveBackStep();
    }
}