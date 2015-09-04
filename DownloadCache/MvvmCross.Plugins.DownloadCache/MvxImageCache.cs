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

namespace MvvmCross.Plugins.DownloadCache
{
    public class MvxImageCache<T>
        : MvxAllThreadDispatchingObject
        , IMvxImageCache<T>
    {
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

            Task.Run(() => {
                Entry entry;
                if (_entriesByHttpUrl.TryGetValue(url, out entry))
                {
                    entry.WhenLastAccessedUtc = DateTime.UtcNow;
                    tcs.TrySetResult(entry.Image.RawImage);
                    return;
                }

                try
                {
                    _fileDownloadCache.RequestLocalFilePath(url, 
                        async s => {
                            var image = await Parse(s).ConfigureAwait(false);
							if(!_entriesByHttpUrl.ContainsKey(url)) {
                            	_entriesByHttpUrl.Add(url, new Entry(url, image));
							}

                            tcs.TrySetResult(image.RawImage);
                        },
                        exception => {
                            tcs.TrySetException(exception);
                        });
                }
                finally
                {
                    ReduceSizeIfNecessary();
                }
            });

            return tcs.Task;
        }

        #endregion

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