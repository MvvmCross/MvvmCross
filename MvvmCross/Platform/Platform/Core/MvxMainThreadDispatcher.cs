// MvxMainThreadDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;
    using System.Reflection;

    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

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