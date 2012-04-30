using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization;
using Phone7.Fx.IO;
using Phone7.Fx.IO.Compression;

namespace Phone7.Fx
{
    /// <summary>
    /// 
    /// </summary>
    public class Cache
    {
        public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;

        readonly IsolatedStorageFile _myStore = IsolatedStorageFile.GetUserStoreForApplication();

        private object _sync = new object();


        private static Cache _current;
        public static Cache Current
        {
            get { return _current ?? (_current = new Cache()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cache use gzip compression in the storage
        /// </summary>
        /// <value><c>true</c> if [use compression]; otherwise, <c>false</c>.</value>
        public bool UseCompression { get; set; }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <param name="slidingExpiration">The sliding expiration.</param>
        public void Add(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            lock (_sync)
            {
                if (Contains(key))
                    Remove(key);

                if (absoluteExpiration == NoAbsoluteExpiration)
                    Add(key, DateTime.UtcNow + slidingExpiration, value);
                if (slidingExpiration == NoSlidingExpiration)
                    Add(key, absoluteExpiration, value);
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="expirationDate">The expiration date.</param>
        /// <param name="value">The value.</param>
        private void Add(string key, DateTime expirationDate, object value)
        {
            lock (_sync)
            {
                if (!_myStore.DirectoryExists(key))
                    _myStore.CreateDirectory(key);
                else
                {
                    string currentFile = GetFileNames(key).FirstOrDefault();
                    if (currentFile != null)
                        _myStore.DeleteFile(string.Format("{0}\\{1}", key, currentFile));
                    _myStore.DeleteDirectory(key);
                    _myStore.CreateDirectory(key);
                }

                string fileName = string.Format("{0}\\{1}.cache", key, expirationDate.ToFileTimeUtc());


                if (_myStore.FileExists(fileName))
                    _myStore.DeleteFile(fileName);

                if (UseCompression)
                    GZipWrite(fileName, value);
                else
                    NormalWrite(fileName, value);
            }

        }

        private void GZipWrite(string fileName, object value)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageGZipFileStream(fileName, FileMode.OpenOrCreate, _myStore, CompressionMode.Compress))
            {
                DataContractSerializer s = new DataContractSerializer(value.GetType());

                s.WriteObject(isolatedStorageFileStream, value);
            }
        }

        private void NormalWrite(string fileName, object value)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.OpenOrCreate, _myStore))
            {
                DataContractSerializer s = new DataContractSerializer(value.GetType());

                s.WriteObject(isolatedStorageFileStream, value);
            }
        }

        /// <summary>
        /// Determines whether the cache contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            lock (_sync)
            {
                if (_myStore.DirectoryExists(key) && GetFileNames(key).Any())
                {
                    string currentFile = GetFileNames(key).FirstOrDefault();
                    if (currentFile != null)
                    {
                        var expirationDate =
                            DateTime.FromFileTimeUtc(long.Parse(Path.GetFileNameWithoutExtension(currentFile)));
                        if (expirationDate >= DateTime.UtcNow)
                            return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            lock (_sync)
            {
                if (!Contains(key))
                    throw new AccessViolationException("The key does not exist in the cache");
                string currentFile = GetFileNames(key).FirstOrDefault();
                if (currentFile != null)
                    _myStore.DeleteFile(string.Format("{0}\\{1}", key, currentFile));
                _myStore.DeleteDirectory(key);
            }
        }

        /// <summary>
        /// Gets the file names.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private IEnumerable<string> GetFileNames(string key)
        {
            return _myStore.GetFileNames(string.Format("{0}\\*.cache", key));
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            lock (_sync)
            {
                string currentFile = GetFileNames(key).FirstOrDefault();
                if (currentFile != null)
                {
                    var expirationDate =
                        DateTime.FromFileTimeUtc(long.Parse(Path.GetFileNameWithoutExtension(currentFile)));
                    if (expirationDate >= DateTime.UtcNow)
                    {
                        if (UseCompression)
                            return GZipRead<T>(string.Format(@"{0}\{1}", key, currentFile));
                        return NormalRead<T>(string.Format(@"{0}\{1}", key, currentFile));
                    }
                    Remove(key);
                }
                return default(T);
            }

        }

        private T NormalRead<T>(string fileName)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, _myStore))
            {
                DataContractSerializer s = new DataContractSerializer(typeof(T));

                var value = s.ReadObject(isolatedStorageFileStream);
                isolatedStorageFileStream.Close();
                return (T)value;
            }
        }

        private T GZipRead<T>(string fileName)
        {
            using (var isolatedStorageFileStream = new IsolatedStorageGZipFileStream(fileName, FileMode.Open, _myStore, CompressionMode.Decompress))
            {
                DataContractSerializer s = new DataContractSerializer(typeof(T));

                var value = s.ReadObject(isolatedStorageFileStream);
                isolatedStorageFileStream.Close();
                return (T)value;
            }
        }

    }
}