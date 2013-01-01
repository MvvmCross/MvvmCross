// IMvxTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
    public interface IMvxTouchViewPresenter
    {
        void Show(MvxShowViewModelRequest view);
        void Close(IMvxViewModel viewModel);
        void CloseModalViewController();
        void ClearBackStack();
        void RequestRemoveBackStep();

        bool PresentModalViewController(UIViewController controller, bool animated);
        void NativeModalViewControllerDisappearedOnItsOwn();
    }
}