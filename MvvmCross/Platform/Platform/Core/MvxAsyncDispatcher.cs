// MvxAsyncDispatcher.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Platform.Core
{
    using System;
    using System.Threading.Tasks;

#warning should really kill this static - replace with IoC please

    public static class MvxAsyncDispatcher
    {
        public static void BeginAsync(Action action)
        {
            Task.Run(action);
        }

        public static void BeginAsync(Action<object> action, object state)
        {
            Task.Run(() => action.Invoke(state));
        }
    }
}