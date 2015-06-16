// MvxImageCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxImageCache<T>
        : MvxAllThreadDispatchingObject
        , IMvxImageCache<T>
    {
        private readonly Dictionary<string, List<TaskCompletionSource<T>>> _currentlyRequested =
            new Dictionary<string, List<TaskCompletionSource<T>>>();

        private readonly Dictionary<string, Entry> _entriesByHttpUrl = new Dictionary<string, Entry>();

        private readonly IMvxFileDownloadCache _fileDownloadCache;
        private readonly int _maxInMemoryBytes;
        private readonly int _maxInMemoryFiles;
        private readonly bool _disposeOnRemove;

        public MvxImageCache(IMvxFileDownloadCache fileDownloadCache, int maxInMemoryFiles, int maxInMemoryBytes, bool disposeOnRemove)
        {
            _fileDownloadCache = fileDownloadCache;
            _maxInMemoryFiles = maxInMemoryFiles;
            _maxInMemoryBytes = maxInMemoryBytes;
            _disposeOnRemove = disposeOnRemove;
        }

        #region IMvxImageCache<T> Members

        public Task<T> RequestImage(string url)
        {
            var tcs = new TaskCompletionSource<T>();

            RunSyncOrAsyncWithLock(async () =>
                {
                    Entry entry;
                    if (_entriesByHttpUrl.TryGetValue(url, out entry))
                    {
                        entry.WhenLastAccessedUtc = DateTime.UtcNow;
                        DoCallback(entry, tcs);
                        return;
                    }

                    List<TaskCompletionSource<T>> currentlyRequested;
                    if (_currentlyRequested.TryGetValue(url, out currentlyRequested))
                    {
                        currentlyRequested.Add(tcs);
                        return;
                    }

                    currentlyRequested = new List<TaskCompletionSource<T>> { tcs };
                    _currentlyRequested[url] = currentlyRequested;

                    try
                    {
                        var stream = await _fileDownloadCache.RequestLocalFilePath(url);
                        await ProcessFilePath(url, stream);
                    }
                    catch (Exception exception)
                    {
                        ProcessError(url, exception);
                    }                    
                });

            return tcs.Task;
        }

        #endregion

        private void DoCallback(Entry entry, TaskCompletionSource<T> tcs)
        {
            tcs.SetResult(entry.Image.RawImage);
        }

        private void DoCallback(Exception exception, TaskCompletionSource<T> tcs)
        {
            tcs.SetException(exception);
        }

        private void ProcessError(string url, Exception exception)
        {
            List<TaskCompletionSource<T>> callbacks = null;
            RunSyncOrAsyncWithLock(
                () =>
                {
                    callbacks = _currentlyRequested[url];
                    _currentlyRequested.Remove(url);
                },
                () =>
                    {
                        foreach (var callback in callbacks)
                        {
                            DoCallback(exception, callback);
                        }
                    }
                );

        }

        private async Task ProcessFilePath(string url, string filePath)
        {
            MvxImage<T> image;
            try
            {
                image = await Parse(filePath);
            }
            catch (Exception exception)
            {
                ProcessError(url, exception);
                return;
            }

            var entry = new Entry(url, image);
            List<TaskCompletionSource<T>> callbacks = null;
            RunSyncOrAsyncWithLock(
                () =>
                {
                    _entriesByHttpUrl[url] = entry;
                    callbacks = _currentlyRequested[url];
                    _currentlyRequested.Remove(url);
                },
                () =>
                {
                    foreach (var callback in callbacks)
                    {
                        DoCallback(entry, callback);
                    }
                    ReduceSizeIfNecessary();
                });
            ;
        }

        private void ReduceSizeIfNecessary()
        {
            RunSyncOrAsyncWithLock(() =>
                {
                    var currentSizeInBytes = _entriesByHttpUrl.Values.Sum(x => x.Image.GetSizeInBytes());
                    var currentCountFiles = _entriesByHttpUrl.Values.Count;

                    if (currentCountFiles <= _maxInMemoryFiles
                        && currentSizeInBytes <= _maxInMemoryBytes)
                        return;

                    // we don't use LINQ OrderBy here because of AOT/JIT problems on MonoTouch
                    List<Entry> sortedEntries = _entriesByHttpUrl.Values.ToList();
                    sortedEntries.Sort(new MvxImageComparer());

                    while (currentCountFiles > _maxInMemoryFiles
                           || currentSizeInBytes > _maxInMemoryBytes)
                    {
                        var toRemove = sortedEntries[0];
                        sortedEntries.RemoveAt(0);

                        currentSizeInBytes -= toRemove.Image.GetSizeInBytes();
                        currentCountFiles--;

                        if (_disposeOnRemove) 
                            toRemove.Image.RawImage.DisposeIfDisposable(); 

                        _entriesByHttpUrl.Remove(toRemove.Url);
                    }
                });
        }

        private class MvxImageComparer : IComparer<Entry>
        {
            public int Compare(Entry x, Entry y)
            {
                return x.WhenLastAccessedUtc.CompareTo(y.WhenLastAccessedUtc);
            }
        }

        protected Task<MvxImage<T>> Parse(string path)
        {
            var loader = Mvx.Resolve<IMvxLocalFileImageLoader<T>>();
            return loader.Load(path, false, 0, 0);
        }

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