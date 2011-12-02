#region Copyright

// <copyright file="MvxTouchViewDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ExtensionMethods;

namespace Cirrious.MvvmCross.Touch.Views
{	
    public class MvxTouchViewDispatcher 
		: MvxTouchUIThreadDispatcher
			, IMvxViewDispatcher
			, IMvxServiceConsumer<IMvxTouchNavigator>
    {
		public bool RequestNavigate(MvxShowViewModelRequest request)
		{
			Action action = () => this.GetService<IMvxTouchNavigator>().NavigateTo(request);
			return InvokeOrBeginInvoke(action);
		}
		
		public bool RequestNavigateBack()
		{
			Action action = () => this.GetService<IMvxTouchNavigator>().GoBack();
			return InvokeOrBeginInvoke(action);
		}
		
		public bool RequestRemoveBackStep()
		{
			// not supported on iOS really
			return false;
		}
    }
}