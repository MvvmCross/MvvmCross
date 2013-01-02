// MvxTouchUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

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