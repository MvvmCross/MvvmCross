// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MvvmCross.ViewModels;

public interface IMvxBundle
{
    IDictionary<string, string> Data { get; }

    void Write(object toStore);

    T? Read<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>() where T : new();

    object? Read([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] Type type);

    IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string? debugText);
}
