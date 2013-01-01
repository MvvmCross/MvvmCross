// MvxBaseTouchViewPresenter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Views;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxBaseTouchViewPresenter
        : IMvxTouchViewPresenter
    {
        #region IMvxTouchViewPresenter Members

        public virtual void Show(MvxShowViewModelRequest view)
        {
        }

        public virtual void CloseModalViewController()
        {
        }

        public virtual void Close(IMvxViewModel viewModel)
        {
        }

        public virtual void ClearBackStack()
        {
        }

        public virtual void RequestRemoveBackStep()
        {
        }

        public virtual bool PresentModalViewController(UIViewController viewController, bool animated)
        {
            return false;
        }

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
        }

        #endregion
    }
}