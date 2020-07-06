// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatcher
        : MvxSingleton<IMvxMainThreadDispatcher>,
        IMvxMainThreadDispatcher
    {
        public abstract void ExecuteOnMainThread(Action action, bool maskExceptions = true);

        public abstract ValueTask ExecuteOnMainThreadAsync(Func<ValueTask> action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }

        protected internal static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (TraceException(ex, maskExceptions))
                    throw;
            }
        }

        protected internal static async ValueTask ExceptionMaskedActionAsync(Func<ValueTask> action, bool maskExceptions)
        {
            try
            {
                await action().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                if (TraceException(ex, maskExceptions))
                    throw;
            }
        }

        private static bool TraceException(Exception exception, bool maskExceptions)
        {
            if(exception is TargetInvocationException targetInvocationException)
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                {
                    MvxLog.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
                    return false;
                }
                else
                    return true;
            }
            else
            {
                MvxLog.Instance.TraceException("Exception throw when invoking action via dispatcher", exception);
                if (maskExceptions)
                {
                    MvxLog.Instance.Warn("Exception masked " + exception.ToLongString());
                    return false;
                }
                else
                    return true;
            }
        }
    }
}
