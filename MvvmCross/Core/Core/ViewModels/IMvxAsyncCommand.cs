// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MvvmCross.Core.ViewModels
{
    public interface IMvxAsyncCommand : IMvxCommand
    {
        Task ExecuteAsync(object parameter = null);
        void Cancel();
    }

    public interface IMvxAsyncCommand<T> : IMvxCommand<T>
    {
        Task ExecuteAsync(T parameter);
        void Cancel();
    }
}