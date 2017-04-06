using System;
using System.Collections.Generic;

namespace MvvmCross.Core.Navigation
{
    public class MvxNavigationCache : IMvxNavigationCache
    {
        private IDictionary<string, object> cache => new Dictionary<string, object>();

        public bool AddOrUpdateValue<T>(string key, T value)
        {
            if (Contains(key))
                return false;
            cache.Add(key, value);
            return true;
        }

        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
        {
            object item;
            bool found = cache.TryGetValue(key, out item);
            if (found)
            {
                return (T)item;
            }
            return defaultValue;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void Clear()
        {
            cache.Clear();
        }

        public bool Contains(string key)
        {
            return cache.ContainsKey(key);
        }
    }
}
