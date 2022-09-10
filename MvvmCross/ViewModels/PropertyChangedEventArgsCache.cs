// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel;

namespace MvvmCross.ViewModels
{
#nullable enable
    /// <summary>
    /// Provides a cache for <see cref="PropertyChangedEventArgs"/> instances.
    /// </summary>
    public sealed class PropertyChangedEventArgsCache
    {
        /// <summary>
        /// The underlying dictionary. This instance is its own mutex.
        /// </summary>
        private readonly Dictionary<string, PropertyChangedEventArgs> _cache =
            new Dictionary<string, PropertyChangedEventArgs>();

        /// <summary>
        /// Private constructor to prevent other instances.
        /// </summary>
        private PropertyChangedEventArgsCache()
        {
        }

        /// <summary>
        /// The global instance of the cache.
        /// </summary>
        public static PropertyChangedEventArgsCache Instance { get; } =
            new PropertyChangedEventArgsCache();

        /// <summary>
        /// Retrieves a <see cref="PropertyChangedEventArgs"/> instance for the specified property, creating it and adding it to the cache if necessary.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        public PropertyChangedEventArgs Get(string propertyName)
        {
            lock (_cache)
            {
                PropertyChangedEventArgs result;
                if (_cache.TryGetValue(propertyName, out result))
                    return result;
                result = new PropertyChangedEventArgs(propertyName);
                _cache.Add(propertyName, result);
                return result;
            }
        }
    }
#nullable restore
}
