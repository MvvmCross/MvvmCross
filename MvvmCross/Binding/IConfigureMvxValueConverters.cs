// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using MvvmCross.Converters;

namespace MvvmCross.Binding;

/// <summary>
/// Implement this interface and register it as a service to have value converters
/// registered into <see cref="IMvxValueConverterRegistry"/> when the binding system initialises.
/// Multiple registrations are supported and all will be applied.
/// </summary>
public interface IConfigureMvxValueConverters
{
    /// <summary>Called during binding initialisation to register value converters.</summary>
    void Register(IMvxValueConverterRegistry registry);
}
