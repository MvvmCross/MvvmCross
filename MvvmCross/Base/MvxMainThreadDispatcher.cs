// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Base
{
    public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>
    {
        protected static void ExceptionMaskedAction(Action action)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxLog.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
            }
            catch (Exception exception)
            {
                // note - all exceptions masked!
                MvxLog.Instance.Warn("Exception masked " + exception.ToLongString());
            }
        }
    }
}
