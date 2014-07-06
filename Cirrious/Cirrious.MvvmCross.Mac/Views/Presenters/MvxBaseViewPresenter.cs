#region Copyright
// <copyright file="MvxBaseTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
using Cirrious.MvvmCross.ViewModels;


#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Mac.Interfaces;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Mac.Views.Presenters
{
    public class MvxBaseViewPresenter 
        : IMvxMacViewPresenter
    {
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
    }
}