// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

#nullable enable
namespace MvvmCross.Binding.Bindings.Target;

public class MvxTargetChangedEventArgs(object? value) : EventArgs
{
    public object? Value { get; } = value;
}
