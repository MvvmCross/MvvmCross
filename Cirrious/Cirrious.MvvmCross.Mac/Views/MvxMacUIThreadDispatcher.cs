// <copyright file="MvxTouchUIThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

using System;
using System.Threading;
using Cirrious.MvvmCross.Interfaces.Views;
using System.Reflection;
using MonoMac.AppKit;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.CrossCore.Exceptions;

namespace Cirrious.MvvmCross.Mac.Views
{
    public abstract class MvxMacUIThreadDispatcher
        : IMvxMainThreadDispatcher
	{
        private bool InvokeOrBeginInvoke(Action action)
        {
			NSApplication.SharedApplication.InvokeOnMainThread(() => {
				try
				{
					action();
				}
                catch (ThreadAbortException)
                {
                    throw;
                }
                catch (TargetInvocationException exception)
				{
                    MvxTrace.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
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