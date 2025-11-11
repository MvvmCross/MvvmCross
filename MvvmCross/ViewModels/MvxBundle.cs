// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using MvvmCross.Core;

namespace MvvmCross.ViewModels;

public class MvxBundle(IDictionary<string, string>? data) : IMvxBundle
{
    public MvxBundle()
        : this(new Dictionary<string, string>())
    {
    }

    public IDictionary<string, string> Data { get; } = data ?? new Dictionary<string, string>();

    public void Write(object toStore)
    {
        ArgumentNullException.ThrowIfNull(toStore);
        Data.Write(toStore);
    }

    public T? Read<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] T>()
        where T : new()
    {
        return Data.Read<T>();
    }

    public object? Read([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)] Type type)
    {
        return Data.Read(type);
    }

    public IEnumerable<object> CreateArgumentList(IEnumerable<ParameterInfo> requiredParameters, string? debugText)
    {
        return Data.CreateArgumentList(requiredParameters, debugText);
    }
}
