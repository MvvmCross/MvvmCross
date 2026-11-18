// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers;

/// <summary>
/// Default implementation of <see cref="IMvxViewTypeRegistry"/>.
/// Stores view types in a case-insensitive dictionary keyed by simple name and full name.
/// </summary>
public class MvxViewTypeRegistry : IMvxViewTypeRegistry
{
    private readonly Dictionary<string, Type> _entries =
        new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc/>
    public void Register(Type viewType)
    {
        ArgumentNullException.ThrowIfNull(viewType);

        _entries[viewType.Name] = viewType;

        if (viewType.FullName is { } fullName)
            _entries[fullName] = viewType;
    }

    /// <inheritdoc/>
    public bool TryResolve(string name, [NotNullWhen(true)] out Type? viewType)
        => _entries.TryGetValue(name, out viewType);
}
