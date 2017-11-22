﻿// MvxMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;

namespace MvvmCross.Platform.Core
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
                MvxSingleton<IMvxLog>.Instance.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
            }
            catch (Exception exception)
            {
                // note - all exceptions masked!
                MvxSingleton<IMvxLog>.Instance.Warn("Exception masked " + exception.ToLongString());
            }
        }
    }
}