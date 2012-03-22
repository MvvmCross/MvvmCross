#region Copyright
// <copyright file="MvxImageCache.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.Core;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform.Images;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxImageCache<T>
        : MvxMainThreadDispatchingObject
        , IMvxImageCache<T>
        , IMvxServiceConsumer<IMvxLocalFileImageLoader<T>>
    {
        private readonly Dictionary<string, List<CallbackPair>> _currentlyRequested = new Dictionary<string, List<CallbackPair>>();
        private readonly Dictionary<string, Entry> _entriesByHttpUrl = new Dictionary<string, Entry>();

        private readonly IMvxFileDownloadCache _fileDownloadCache;
        private readonly int _maxInMemoryBytes;
        private readonly int _maxInMemoryFiles;

        public MvxImageCache(IMvxFileDownloadCache fileDownloadCache, int maxInMemoryFiles, int maxInMemoryBytes)
        {
            _fileDownloadCache = fileDownloadCache;
            _maxInMemoryFiles = maxInMemoryFiles;
            _maxInMemoryBytes = maxInMemoryBytes;
        }

        #region IMvxImageCache<T> Members

        public void RequestImage(string url, Action<T> success, Action<Exception> error)
        {
            MvxAsyncDispatcher.BeginAsync(() => DoRequestImage(url, success, error));
        }

        #endregion

        private void DoRequestImage(string url, Action<T> success, Action<Exception> error)
        {
            lock (this)
            {
                Entry entry;
                if (_entriesByHttpUrl.TryGetValue(url, out entry))
                {
                    entry.WhenLastAccessedUtc = DateTime.UtcNow;
                    DoCallback(entry, success);
                    return;
                }

                List<CallbackPair> currentlyRequested;
                if (_currentlyRequested.TryGetValue(url, out currentlyRequested))
                {
                    currentlyRequested.Add(new CallbackPair(success,error));
                    return;
                }

                currentlyRequested = new List<CallbackPair> {new CallbackPair(success, error)};
                _currentlyRequested[url] = currentlyRequested;

                _fileDownloadCache.RequestLocalFilePath(url, (stream) => ProcessFilePath(url, stream), (exception) => ProcessError(url,exception));
            }
        }

        private void DoCallback(Entry entry, Action<T> success)
        {
            InvokeOnMainThread(() => success(entry.Image.RawImage));
        }

        private void DoCallback(Exception exception, Action<Exception> error)
        {
            InvokeOnMainThread(() => error(exception));
        }

        private void ProcessError(string url, Exception exception)
        {
            List<CallbackPair> callbackPairs;
            lock (this)
            {
                callbackPairs = _currentlyRequested[url];
                _currentlyRequested.Remove(url);
            }

            foreach (var callbackPair in callbackPairs)
            {
                DoCallback(exception, callbackPair.Error);
            }
        }

        private void ProcessFilePath(string url, string filePath)
        {
            MvxImage<T> image;

            try
            {
                image = Parse(filePath);
            }
#if !NETFX_CORE
            catch (ThreadAbortException)
            {
                throw;
            }
#endif 
            catch (Exception exception)
            {
                ProcessError(url, exception);
                return;
            }

            var entry = new Entry(url, image);
            List<CallbackPair> callbackPairs;
            lock (this)
            {
                _entriesByHttpUrl[url] = entry;
                callbackPairs = _currentlyRequested[url];
                _currentlyRequested.Remove(url);
            }
            foreach (var callbackPair in callbackPairs)
            {
                DoCallback(entry, callbackPair.Success);
            }
            ReduceSizeIfNecessary();
        }

        private void ReduceSizeIfNecessary()
        {
            lock (this)
            {
                var currentSizeInBytes = _entriesByHttpUrl.Values.Sum(x => x.Image.GetSizeInBytes());
                var currentCountFiles = _entriesByHttpUrl.Values.Count;

                if (currentCountFiles <= _maxInMemoryFiles
                    && currentSizeInBytes <= _maxInMemoryBytes)
                    return;
				
				// we don't use LINQ OrderBy here because of AOT/JIT problems on MonoTouch
                List<Entry> sortedEntries = _entriesByHttpUrl.Values.ToList();
				sortedEntries.Sort((a,b) => a.WhenLastAccessedUtc.CompareTo(b.WhenLastAccessedUtc));

                while (currentCountFiles > _maxInMemoryFiles
                    || currentSizeInBytes > _maxInMemoryBytes)
                {
                    var toRemove = sortedEntries[0];
                    sortedEntries.RemoveAt(0);

                    currentSizeInBytes -= toRemove.Image.GetSizeInBytes();
                    currentCountFiles--;

                    _entriesByHttpUrl.Remove(toRemove.Url);
                }
            }
        }

        protected MvxImage<T> Parse(string path)
        {
            var loader = this.GetService<IMvxLocalFileImageLoader<T>>();
            return loader.Load(path, false);
        }

        #region Nested type: CallbackPair

        private class CallbackPair
        {
            public CallbackPair(Action<T> success, Action<Exception> error)
            {
                Error = error;
                Success = success;
            }

            public Action<T> Success { get; private set; }
            public Action<Exception> Error { get; private set; }
        }

        #endregion

        #region Nested type: Entry

        private class Entry
        {
            public Entry(string url, MvxImage<T> image)
            {
                Url = url;
                Image = image;
                WhenLastAccessedUtc = DateTime.UtcNow;
            }

            public string Url { get; private set; }
            public MvxImage<T> Image { get; private set; }
            public DateTime WhenLastAccessedUtc { get; set; }
        }

        #endregion
    }
}