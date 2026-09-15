// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Bindings.Source.Construction;

namespace MvvmCross.Plugin.FieldBinding;

/// <summary>
/// Extension methods for registering the MvvmCross FieldBinding plugin.
/// </summary>
public static class MvxFieldBindingServiceCollectionExtensions
{
    /// <summary>
    /// Registers a hosted service that adds <see cref="MvxFieldSourceBindingFactoryExtension"/>
    /// to <see cref="IMvxSourceBindingFactoryExtensionHost"/> when the binding system initialises.
    /// </summary>
    [RequiresUnreferencedCode("MvxFieldBinding requires unreferenced code")]
    public static IServiceCollection AddMvxFieldBinding(this IServiceCollection services)
    {
        services.AddSingleton<IMvxSourceBindingFactoryExtension, MvxFieldSourceBindingFactoryExtension>();
        return services;
    }
}
