// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Windows.Input;

namespace MvvmCross.Commands;

public interface IMvxCommand : ICommand
{
    void RaiseCanExecuteChanged();

    void Execute();

    bool CanExecute();
}

public interface IMvxCommand<in TParameter> : ICommand
{
    void Execute(TParameter parameter);

    bool CanExecute(TParameter parameter);

    void RaiseCanExecuteChanged();
}
