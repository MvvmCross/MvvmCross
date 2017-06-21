using System.Collections.Generic;

namespace MvvmCross.Core.Navigation
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
