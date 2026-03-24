// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Microsoft.Extensions.DependencyInjection;
using MvvmCross.Binding.Bindings.Source.Construction;

namespace MvvmCross.Plugin.MethodBinding;

/// <summary>
/// Extension methods for registering the MvvmCross MethodBinding plugin.
/// </summary>
public static class MvxMethodBindingServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="MvxMethodSourceBindingFactoryExtension"/> into the binding system.
    /// </summary>
    public static IServiceCollection AddMvvmCrossMethodBinding(this IServiceCollection services)
    {
        services.AddSingleton<IMvxSourceBindingFactoryExtension, MvxMethodSourceBindingFactoryExtension>();
        return services;
    }
}
