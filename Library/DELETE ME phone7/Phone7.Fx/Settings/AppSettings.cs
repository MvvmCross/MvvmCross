using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace Phone7.Fx.Settings
{
    public class AppSettings
    {
        private static readonly IsolatedStorageSettings _isolatedStore;

        static AppSettings()
        {
            try
            {
                // Get the settings for this application.
                _isolatedStore = IsolatedStorageSettings.ApplicationSettings;

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while using IsolatedStorageSettings: " + e.ToString());
            }
        }

        public static void AddOrUpdateValue(string key, Object value)
        {
            // If the key exists
            if (_isolatedStore.Contains(key))
            {
                // If the value has changed
                if (_isolatedStore[key] != value)
                {
                    // Store the new value
                    _isolatedStore[key] = value;
                }
            }
            // Otherwise create the key.
            else
            {
                _isolatedStore.Add(key, value);

            }
            _isolatedStore.Save();
        }

        public static bool Remove(string key)
        {
            if (_isolatedStore.Contains(key))
            {
                return _isolatedStore.Remove(key);
            }
            return false;
        }

        public static T GetValueOrDefault<T>(string key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (_isolatedStore.Contains(key))
            {
                value = (T)_isolatedStore[key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }
    }
}