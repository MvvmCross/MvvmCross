﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace MvvmCross.Navigation
{
    public class MvxNavigationCache : IMvxNavigationCache
    {
        private IDictionary<string, object> Cache { get; } = new Dictionary<string, object>();

        public bool AddValue<T>(string key, T value)
        {
            if (Contains(key))
                return false;
            Cache.Add(key, value);
            return true;
        }

        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            if (Cache.TryGetValue(key, out object item))
                return (T)item;

            return defaultValue;
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Clear()
        {
            Cache.Clear();
        }

        public bool Contains(string key)
        {
            return Cache.ContainsKey(key);
        }
    }
}
