// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
namespace MvvmCross.Commands;

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
