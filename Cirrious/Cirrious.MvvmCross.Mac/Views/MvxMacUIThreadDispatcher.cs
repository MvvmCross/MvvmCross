// MvxMacUIThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Threading;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.Core;
using Cirrious.CrossCore.Platform.Diagnostics;

namespace Cirrious.MvvmCross.Touch.Views
{
    public abstract class MvxMacUIThreadDispatcher
        : IMvxMainThreadDispatcher
    {
        private bool InvokeOrBeginInvoke(Action action)
        {
#warning _stopRequested removed			
            //if (_stopRequested)
            //    return false;

            NSApplication.SharedApplication.InvokeOnMainThread(() =>
                {
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