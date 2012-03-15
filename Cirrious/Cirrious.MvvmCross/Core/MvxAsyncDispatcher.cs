#region Copyright
// <copyright file="MvxAsyncDispatcher.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
#if NETFX_CORE
using Windows.System.Threading;
#else
using System.Threading;
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

        public static void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        public static void Sleep(TimeSpan timeSpan)
        {
            Thread.Sleep(timeSpan);
        }
    }
#endif
}