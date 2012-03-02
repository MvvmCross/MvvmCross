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
using System.Threading;

namespace Cirrious.MvvmCross.Core
{
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
}