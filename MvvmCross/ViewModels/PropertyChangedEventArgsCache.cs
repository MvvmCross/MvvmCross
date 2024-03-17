// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.ComponentModel;

namespace MvvmCross.ViewModels;

/// <summary>
/// Provides a cache for <see cref="PropertyChangedEventArgs"/> instances.
/// </summary>
public sealed class PropertyChangedEventArgsCache
{
    private readonly object _lock = new();

    /// <summary>
    /// The underlying dictionary. This instance is its own mutex.
    /// </summary>
    private readonly Dictionary<string, PropertyChangedEventArgs> _cache = new();

    /// <summary>
    /// Private constructor to prevent other instances.
    /// </summary>
    private PropertyChangedEventArgsCache()
    {
    }

    /// <summary>
    /// The global instance of the cache.
    /// </summary>
    public static PropertyChangedEventArgsCache Instance { get; } = new();

    /// <summary>
    /// Retrieves a <see cref="PropertyChangedEventArgs"/> instance for the specified property, creating it and adding it to the cache if necessary.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public PropertyChangedEventArgs Get(string propertyName)
    {
        lock (_lock)
        {
            if (_cache.TryGetValue(propertyName, out var result))
                return result;
            result = new PropertyChangedEventArgs(propertyName);
            _cache.Add(propertyName, result);
            return result;
        }
    }
}
