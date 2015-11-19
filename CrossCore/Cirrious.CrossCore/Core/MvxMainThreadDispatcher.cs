// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using System;
using System.Reflection;

namespace Cirrious.CrossCore.Core
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
                MvxTrace.Trace("TargetInvocateException masked " + exception.InnerException.ToLongString());
            }
            catch (Exception exception)
            {
                // note - all exceptions masked!
                MvxTrace.Warning("Exception masked " + exception.ToLongString());
            }
        }
    }
}