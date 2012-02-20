#region Copyright

// <copyright file="MvxTouchUIThreadDispatcher.cs" company="Cirrious">
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
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Views;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxTouchUIThreadDispatcher : IMvxMainThreadDispatcher
	{
        protected bool InvokeOrBeginInvoke(Action action)
        {
#warning _stopRequested removed			
            //if (_stopRequested)
            //    return false;
			
			MonoTouch.UIKit.UIApplication.SharedApplication.InvokeOnMainThread(() => {
	            //if (_stopRequested)
	            //    return;
				
				try
				{
					action();
				}
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (Exception exception)
				{
#warning Should we mask all these exceptions?
                    MvxTrace.Trace("Exception masked " + exception.ToLongString());
				}
			});
            return true;
        }

		#region IMvxMainThreadDispatcher implementation
		
		public bool RequestMainThreadAction(Action action)
		{
			return InvokeOrBeginInvoke(action);
		}
		
		#endregion
	}	
}