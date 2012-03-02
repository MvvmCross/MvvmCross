#region Copyright
// <copyright file="MvxBaseTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Touch.Interfaces;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxBaseTouchViewPresenter : IMvxTouchViewPresenter
    {
        #region IMvxTouchViewPresenter Members

        public virtual bool ShowView(IMvxTouchView view)
        {
            return false;
        }

        public virtual bool GoBack()
        {
            return false;
        }

        public virtual void ClearBackStack()
        {
        }

        public virtual bool PresentNativeModalViewController(UIViewController viewController, bool animated)
        {
            return false;
        }

        #endregion
    }
}