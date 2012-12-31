#region Copyright

// <copyright file="MvxTouchUIThreadDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using System.Reflection;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Platform.Diagnostics;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxTouchUIThreadDispatcher
        : IMvxMainThreadDispatcher
    {
        private bool InvokeOrBeginInvoke(Action action)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
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
                        // note - all exceptions masked!
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