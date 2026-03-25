// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

/// <summary>
/// A registry of Android <see cref="View"/> types that MvvmCross can resolve by tag name
/// during binding inflation. Types must be registered explicitly — no assembly scanning is performed.
/// </summary>
/// <remarks>
/// Register custom or framework view types via the
/// <c>services.AddMvvmCrossAndroidViewType&lt;TView&gt;()</c> extension method on
/// <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
/// </remarks>
public interface IMvxViewTypeRegistry
{
    /// <summary>Registers a view type under its simple name and fully-qualified name.</summary>
    void Register(Type viewType);

    /// <summary>
    /// Attempts to resolve a view type by the given name. The name is matched
    /// case-insensitively against both simple names and fully-qualified names.
    /// </summary>
    bool TryResolve(string name, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Type? viewType);
}
