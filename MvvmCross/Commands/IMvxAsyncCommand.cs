// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MvvmCross.Commands
{
#nullable enable
    public interface IMvxAsyncCommand : IMvxCommand
    {
        Task ExecuteAsync(object? parameter = null);
        void Cancel();
    }

    public interface IMvxAsyncCommand<in TParameter> : IMvxCommand<TParameter>
    {
        Task ExecuteAsync(TParameter parameter);
        void Cancel();
    }
#nullable restore
}
