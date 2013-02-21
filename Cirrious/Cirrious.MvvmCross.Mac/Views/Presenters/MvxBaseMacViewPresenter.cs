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

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views.Presenters
{
    public class MvxBaseMacViewPresenter 
        : IMvxMacViewPresenter
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

        public virtual void NativeModalViewControllerDisappearedOnItsOwn()
        {
            // ignored
        }

        #endregion
    }
}