// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;

namespace MvvmCross.Binding.Bindings.Target;

public interface IMvxTargetBinding : IMvxBinding
{
    event EventHandler<MvxTargetChangedEventArgs>? ValueChanged;

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    Type TargetValueType { get; }
    MvxBindingMode DefaultMode { get; }

    void SetValue(object? value);

    void SubscribeToEvents();
}
