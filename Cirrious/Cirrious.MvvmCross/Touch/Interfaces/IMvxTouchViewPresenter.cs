#region Copyright

// <copyright file="IMvxTouchViewPresenter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Touch.Interfaces
{
	public interface IMvxTouchViewPresenter
	{
		void ShowView(IMvxTouchView view);
		void GoBack();
	}
}