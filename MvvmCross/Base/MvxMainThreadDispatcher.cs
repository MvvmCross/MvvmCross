// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
#nullable enable
    public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>, IMvxMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxLog.Instance?.TraceException("Exception thrown when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance?.Trace("TargetInvocationException masked " + exception.InnerException.ToLongString());
                else
                    throw;
            }
            catch (Exception exception)
            {
                MvxLog.Instance?.TraceException("Exception thrown when invoking action via dispatcher", exception);
                if (maskExceptions)
                    MvxLog.Instance?.Warn("Exception masked " + exception.ToLongString());
                else
                    throw;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
#nullable restore
}
