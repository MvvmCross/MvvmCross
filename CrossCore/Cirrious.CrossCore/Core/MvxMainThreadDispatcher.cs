// MvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Reflection;
using System.Threading.Tasks;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;

namespace Cirrious.CrossCore.Core
{
    public abstract class MvxMainThreadDispatcher : MvxSingleton<IMvxMainThreadDispatcher>
    {
        public Task RunOnBackgroundThread(Action action)
        {
            if (IsInMainThread())
            {
                return Task.Run(action);
            }
            else
            {
                action();
                return Task.FromResult(true);
            }
        }

        public Task<T> RunOnBackgroundThread<T>(Func<T> func)
        {
            if (IsInMainThread())
            {
                return Task.Run(func);
            }
            else
            {
                return Task.FromResult(func());
            }
        }

        public Task RunOnBackgroundThread(Func<Task> asyncAction)
        {
            return IsInMainThread() ? Task.Run(asyncAction) : asyncAction();
        }

        public Task<T> RunOnBackgroundThread<T>(Func<Task<T>> asyncFunc)
        {
            return IsInMainThread() ? Task.Run(asyncFunc) : asyncFunc();
        }

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

        protected abstract bool IsInMainThread();
    }
}