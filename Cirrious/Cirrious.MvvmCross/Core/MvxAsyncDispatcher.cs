// MvxAsyncDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading;

#if NETFX_CORE
using Windows.System.Threading;
#else

#endif

namespace Cirrious.MvvmCross.Core
{
#if NETFX_CORE
    public static class MvxAsyncDispatcher
    {
        public static void BeginAsync(Action action)
        {
            ThreadPool.RunAsync((ignored) => action());
        }

        public static void BeginAsync(Action<object> action, object state)
        {
            ThreadPool.RunAsync(ignored => action(state));
        }

        public static void Sleep(int milliseconds)
        {
            Sleep(TimeSpan.FromMilliseconds(milliseconds));
        }

        public static void Sleep(TimeSpan timeSpan)
        {
            new System.Threading.ManualResetEvent(false).WaitOne(timeSpan);
        }
    }
#else
    public static class MvxAsyncDispatcher
    {
        public static void BeginAsync(Action action)
        {
            ThreadPool.QueueUserWorkItem(ignored => action());
        }

        public static void BeginAsync(Action<object> action, object state)
        {
            ThreadPool.QueueUserWorkItem(arg => action(arg), state);
        }
    }
#endif
}