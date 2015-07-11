// CountingMockMainThreadDispatcher.cs
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
    public class CountingMockMainThreadDispatcher
        : MvxSingleton<IMvxMainThreadDispatcher>
          , IMvxMainThreadDispatcher
    {
        public int Count { get; set; }

        public bool RequestMainThreadAction(Action action)
        {
            Count++;
            return true;
        }

        public Task RunOnBackgroundThread(Action action)
        {
            Count++;
            return Task.FromResult(true);
        }

        public Task<T> RunOnBackgroundThread<T>(Func<T> func)
        {
            Count++;
            return Task.FromResult(default(T));
        }

        public Task RunOnBackgroundThread(Func<Task> asyncAction)
        {
            Count++;
            return Task.FromResult(true);
        }

        public Task<T> RunOnBackgroundThread<T>(Func<Task<T>> asyncFunc)
        {
            Count++;
            return Task.FromResult(default(T));
        }
    }
}