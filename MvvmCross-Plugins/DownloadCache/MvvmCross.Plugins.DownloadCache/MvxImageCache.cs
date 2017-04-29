// MvxImageCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace MvvmCross.Plugins.DownloadCache
{
    [Preserve(AllMembers = true)]
    public class MvxImageCache<T>
        : MvxAllThreadDispatchingObject
            , IMvxImageCache<T>
    {
        private readonly bool _disposeOnRemove;

        private readonly IMvxFileDownloadCache _fileDownloadCache;
        private readonly int _maxInMemoryBytes;
        private readonly int _maxInMemoryFiles;
        private ImmutableDictionary<string, Entry> _entriesByHttpUrl = ImmutableDictionary.Create<string, Entry>();

        public MvxImageCache(IMvxFileDownloadCache fileDownloadCache, int maxInMemoryFiles, int maxInMemoryBytes,
            bool disposeOnRemove)
        {
            _fileDownloadCache = fileDownloadCache;
            _maxInMemoryFiles = maxInMemoryFiles;
            _maxInMemoryBytes = maxInMemoryBytes;
            _disposeOnRemove = disposeOnRemove;
        }

        private void ReduceSizeIfNecessary()
        {
            RunSyncOrAsyncWithLock(() =>
            {
                var entries = _entriesByHttpUrl.Select(kvp => kvp.Value).ToList();

                var currentSizeInBytes = entries.Sum(x => x.Image.GetSizeInBytes());
                var currentCountFiles = entries.Count;

                if (currentCountFiles <= _maxInMemoryFiles
                    && currentSizeInBytes <= _maxInMemoryBytes)
                    return;

                // we don't use LINQ OrderBy here because of AOT/JIT problems on MonoTouch
                entries.Sort(new MvxImageComparer());

                var entriesToRemove = new List<Entry>();

                while (currentCountFiles > _maxInMemoryFiles
                       || currentSizeInBytes > _maxInMemoryBytes)
                {
                    var toRemove = entries[0];
                    entries.RemoveAt(0);

                    entriesToRemove.Add(toRemove);

                    currentSizeInBytes -= toRemove.Image.GetSizeInBytes();
                    currentCountFiles--;

                    _entriesByHttpUrl = _entriesByHttpUrl.Remove(toRemove.Url);
                }

                if (_disposeOnRemove && entriesToRemove.Count > 0)
                    DisposeImagesOnMainThread(entriesToRemove);
            });
        }

        private void DisposeImagesOnMainThread(List<Entry> entries)
        {
            InvokeOnMainThread(() =>
            {
                foreach (var entry in entries)
                    entry.Image.RawImage.DisposeIfDisposable();
            });
        }

        protected Task<MvxImage<T>> Parse(string path)
        {
            var loader = Mvx.Resolve<IMvxLocalFileImageLoader<T>>();
            return loader.Load(path, false, 0, 0);
        }

        private class MvxImageComparer : IComparer<Entry>
        {
            public int Compare(Entry x, Entry y)
            {
                return x.WhenLastAccessedUtc.CompareTo(y.WhenLastAccessedUtc);
            }
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

            public string Url { get; }
            public MvxImage<T> Image { get; }
            public DateTime WhenLastAccessedUtc { get; set; }
        }

        #endregion Nested type: Entry

        #region IMvxImageCache<T> Members

        public bool ContainsImage(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return _entriesByHttpUrl.ContainsKey(url);
        }

        public Task<T> RequestImage(string url)
        {
            var tcs = new TaskCompletionSource<T>();

            Task.Run(() =>
            {
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
                        async s =>
                        {
                            var image = await Parse(s).ConfigureAwait(false);
                            _entriesByHttpUrl = _entriesByHttpUrl.SetItem(url, new Entry(url, image));
                            tcs.TrySetResult(image.RawImage);
                        },
                        exception => { tcs.TrySetException(exception); });
                }
                finally
                {
                    ReduceSizeIfNecessary();
                }
            });

            return tcs.Task;
        }

        #endregion IMvxImageCache<T> Members
    }
}