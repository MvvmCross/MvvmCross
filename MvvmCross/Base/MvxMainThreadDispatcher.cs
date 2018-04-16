// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
    {
        protected static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
                else
                    throw exception;
            }
            catch (Exception exception)
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance.Warn("Exception masked " + exception.ToLongString());
                else
                    throw exception;
            }
        }

        public virtual void RequestMainThreadAction(Action action, bool maskExceptions = true) { }
    }
}
