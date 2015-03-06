// IMvxMainThreadDispatcher.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Threading.Tasks;

namespace Cirrious.CrossCore.Core
{
    public interface IMvxMainThreadDispatcher
    {
        bool RequestMainThreadAction(Action action);

        Task RunOnBackgroundThread(Action action);

        Task<T> RunOnBackgroundThread<T>(Func<T> func);

        Task RunOnBackgroundThread(Func<Task> asyncAction);

        Task<T> RunOnBackgroundThread<T>(Func<Task<T>> asyncFunc);
    }
}