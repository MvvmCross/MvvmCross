// InlineMockMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Test.Mocks.Dispatchers
{
    public class InlineMockMainThreadDispatcher
        : MvxSingleton<IMvxMainThreadDispatcher>
          , IMvxMainThreadDispatcher
    {
        public virtual bool RequestMainThreadAction(Action action)
        {
            action();
            return true;
        }

        public Task RunOnBackgroundThread(Action action)
        {
            action();
            return Task.FromResult(true);
        }

        public Task<T> RunOnBackgroundThread<T>(Func<T> func)
        {
            return Task.FromResult(func());
        }

        public Task RunOnBackgroundThread(Func<Task> asyncAction)
        {
            return asyncAction();
        }

        public Task<T> RunOnBackgroundThread<T>(Func<Task<T>> asyncFunc)
        {
            return asyncFunc();
        }
    }
}