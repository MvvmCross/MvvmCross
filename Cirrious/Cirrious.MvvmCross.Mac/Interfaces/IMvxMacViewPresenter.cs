#region Copyright
// <copyright file="IMvxTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;
using MonoMac.AppKit;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
	public interface IMvxMacViewPresenter
	{
		void Show(MvxShowViewModelRequest view);
        void Close(IMvxViewModel viewModel);
	    void ClearBackStack();
	}
}