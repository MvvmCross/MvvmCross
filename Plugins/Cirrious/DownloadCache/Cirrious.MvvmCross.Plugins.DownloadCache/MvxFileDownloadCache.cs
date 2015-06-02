// MvxFileDownloadCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.File;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxFileDownloadCache
        : MvxLockableObject
        , IMvxFileDownloadCache
    {
        private const string CacheIndexFileName = "_CacheIndex.txt";
        private static readonly TimeSpan PeriodSaveInterval = TimeSpan.FromSeconds(1.0);

        private IMvxTextSerializer _textConvert;
        private bool _textConvertTried;
        protected IMvxTextSerializer TextConvert
        {
            get
            {
                if (_textConvert != null)
                    return _textConvert;

                if (_textConvertTried)
                    return null;

                _textConvertTried = true;
                if (!Mvx.TryResolve<IMvxTextSerializer>(out _textConvert))
                    Mvx.Warning("Persistent download cache will not be available - no text serializer available");

                return _textConvert;
            }
        }

        public class Entry
        {
            public string HttpSource { get; set; }
            public string DownloadedPath { get; set; }
            public DateTime WhenLastAccessedUtc { get; set; }
            public DateTime WhenDownloadedUtc { get; set; }
        }

        private readonly string _cacheFolder;
        private readonly string _cacheName;

        private readonly int _maxFileCount;
        private readonly TimeSpan _maxFileAge;

        private readonly Dictionary<string, Entry> _entriesByHttpUrl;

        private readonly Dictionary<string, List<CallbackPair>> _currentlyRequested =
            new Dictionary<string, List<CallbackPair>>();

        private class CallbackPair
        {
            public CallbackPair(Action<string> success, Action<Exception> error)
            {
                Error = error;
                Success = success;
            }

            public Action<string> Success { get; private set; }
            public Action<Exception> Error { get; private set; }
        }

        private readonly List<string> _toDeleteFiles = new List<string>();

        private readonly Timer _periodicTaskTimer;
        private bool _indexNeedsSaving;

        private string IndexFilePath
        {
            get
            {
                var fileService = MvxFileStoreHelper.SafeGetFileStore();
                return fileService.PathCombine(_cacheFolder, _cacheName + CacheIndexFileName);
            }
        }

        public MvxFileDownloadCache(string cacheName, string cacheFolder, int maxFileCount, TimeSpan maxFileAge)
        {
            _cacheName = cacheName;
            _cacheFolder = cacheFolder;
            _maxFileCount = maxFileCount;
            _maxFileAge = maxFileAge;

            EnsureCacheFolderExists();
            _entriesByHttpUrl = LoadIndexEntries();

            QueueUnindexedFilesForDelete();
            QueueOutOfDateFilesForDelete();

            _indexNeedsSaving = false;
            _periodicTaskTimer = new Timer((ignored) => DoPeriodicTasks(), null, PeriodSaveInterval, PeriodSaveInterval);
        }

        #region Constructor helper methods

        private void QueueOutOfDateFilesForDelete()
        {
            var now = DateTime.UtcNow;
            var toRemove = _entriesByHttpUrl.Values.Where(x => (now - x.WhenDownloadedUtc) > _maxFileAge).ToList();
            foreach (var entry in toRemove)
            {
                _entriesByHttpUrl.Remove(entry.HttpSource);
            }
            _toDeleteFiles.AddRange(toRemove.Select(x => x.DownloadedPath));
        }

        private void QueueUnindexedFilesForDelete()
        {
            var store = MvxFileStoreHelper.SafeGetFileStore();
            var files = store.GetFilesIn(_cacheFolder);

            // we don't use Linq because of AOT/JIT problem on MonoTouch :/
            //var cachedFiles = _entriesByHttpUrl.ToDictionary(x => x.Value.DownloadedPath);
            var cachedFiles = new Dictionary<string, Entry>();
            foreach (var e in _entriesByHttpUrl)
            {
                cachedFiles[store.NativePath(e.Value.DownloadedPath)] = e.Value;
            }

            var toDelete = new List<string>();
            foreach (var file in files)
            {
                if (!cachedFiles.ContainsKey(file))
                    if (!file.EndsWith(CacheIndexFileName))
                        toDelete.Add(file);
            }

            RunSyncOrAsyncWithLock(() =>
                {
                    _toDeleteFiles.AddRange(toDelete);
                });
        }

        private void EnsureCacheFolderExists()
        {
            var store = MvxFileStoreHelper.SafeGetFileStore();
            store.EnsureFolderExists(_cacheFolder);
        }

        private Dictionary<string, Entry> LoadIndexEntries()
        {
            try
            {
                var store = MvxFileStoreHelper.SafeGetFileStore();
                string text;
                if (store.TryReadTextFile(IndexFilePath, out text))
                {
                    var textConvert = TextConvert;
                    if (textConvert == null)
                        return new Dictionary<string, Entry>();

                    var list = textConvert.DeserializeObject<List<Entry>>(text);
                    return list.ToDictionary(x => x.HttpSource, x => x);
                }
            }
                //catch (ThreadAbortException)
                //{
                //    throw;
                //}
            catch (Exception exception)
            {
                MvxTrace.Warning( "Failed to read cache index {0} - reason {1}", _cacheFolder,
                               exception.ToLongString());
            }

            return new Dictionary<string, Entry>();
        }

        #endregion

        #region Periodic Tasks

        private void DoPeriodicTasks()
        {
            SaveIndexIfDirty();
            DeleteOldestOneIfTooManyFiles();
            DeleteNextUnneededFile();
        }

        private void DeleteOldestOneIfTooManyFiles()
        {
            RunSyncWithLock(() =>
                {
                    if (_entriesByHttpUrl.Count <= _maxFileCount)
                        return;

                    var nextToDelete = _entriesByHttpUrl.Values.First();
                    foreach (var entry in _entriesByHttpUrl.Values)
                    {
                        if (entry.WhenLastAccessedUtc < nextToDelete.WhenLastAccessedUtc)
                            nextToDelete = entry;
                    }
                    _entriesByHttpUrl.Remove(nextToDelete.HttpSource);
                    _toDeleteFiles.Add(nextToDelete.DownloadedPath);
                });
        }

        private void DeleteNextUnneededFile()
        {
            string nextFileToDelete = "";
            RunSyncWithLock(() =>
                {
                    nextFileToDelete = _toDeleteFiles.FirstOrDefault();
                });

            if (string.IsNullOrEmpty(nextFileToDelete))
                return;

            try
            {
                var fileService = MvxFileStoreHelper.SafeGetFileStore();
                if (fileService.Exists(nextFileToDelete))
                    fileService.DeleteFile(nextFileToDelete);
            }
            catch (Exception exception)
            {
                MvxTrace.Warning( "Problem seen deleting file {0} problem {1}", nextFileToDelete,
                               exception.ToLongString());
            }
        }

        private void SaveIndexIfDirty()
        {
            if (!_indexNeedsSaving)
                return;

            List<Entry> toSave = null;
            RunSyncWithLock(() =>
                {
                    toSave = _entriesByHttpUrl.Values.ToList();
                    _indexNeedsSaving = false;
                });

            try
            {
                var textConvert = TextConvert;
                if (textConvert == null)
                    return;
                var text = TextConvert.SerializeObject(toSave);

                var store = MvxFileStoreHelper.SafeGetFileStore();
                store.WriteFile(IndexFilePath, text);
            }
            catch (Exception exception)
            {
                MvxTrace.Warning( "Failed to save cache index {0} - reason {1}", _cacheFolder,
                               exception.ToLongString());
            }
        }

        #endregion

        public void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error)
        {
            Task.Run(() => DoRequestLocalFilePath(httpSource, success, error));
        }

        private void DoRequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error)
        {
            RunSyncOrAsyncWithLock(() =>
                {
                    Entry diskEntry;
                    if (_entriesByHttpUrl.TryGetValue(httpSource, out diskEntry))
                    {
                        var service = MvxFileStoreHelper.SafeGetFileStore();
                        if (!service.Exists(diskEntry.DownloadedPath))
                        {
                            _entriesByHttpUrl.Remove(httpSource);
                        }
                        else
                        {
                            diskEntry.WhenLastAccessedUtc = DateTime.UtcNow;
                            DoFilePathCallback(diskEntry, success, error);
                            return;
                        }
                    }

                    List<CallbackPair> currentlyRequested;
                    if (_currentlyRequested.TryGetValue(httpSource, out currentlyRequested))
                    {
                        currentlyRequested.Add(new CallbackPair(success, error));
                        return;
                    }

                    currentlyRequested = new List<CallbackPair>
                        {
                            new CallbackPair(success, error)
                        };
                    _currentlyRequested.Add(httpSource, currentlyRequested);
                    var downloader = Mvx.Resolve<IMvxHttpFileDownloader>();
                    var fileService = MvxFileStoreHelper.SafeGetFileStore();
                    var pathForDownload = fileService.PathCombine(_cacheFolder, Guid.NewGuid().ToString("N"));
                    downloader.RequestDownload(httpSource, pathForDownload,
                                               () => OnDownloadSuccess(httpSource, pathForDownload),
                                               (exception) => OnDownloadError(httpSource, exception));
                });
        }
        
        public void ClearAll()
        {
            RunSyncWithLock(() =>
                {
                    var service = MvxFileStoreHelper.SafeGetFileStore();
                    foreach (var entries in _entriesByHttpUrl)
                    {
                        service.DeleteFile(entries.Value.DownloadedPath);
                    }
                    _entriesByHttpUrl.Clear();
                    _indexNeedsSaving = true;
                });
        }

        public void Clear(string httpSource)
        {
            RunSyncWithLock(() =>
                {
                    Entry diskEntry;
                    if (_entriesByHttpUrl.TryGetValue(httpSource, out diskEntry))
                    {
                        _toDeleteFiles.Add(diskEntry.DownloadedPath);
                        _entriesByHttpUrl.Remove(httpSource);
                        _indexNeedsSaving = true;
                    }
                });
        }

        private void OnDownloadSuccess(string httpSource, string pathForDownload)
        {
            RunSyncOrAsyncWithLock(() =>
                {
                    var diskEntry = new Entry
                        {
                            DownloadedPath = pathForDownload,
                            HttpSource = httpSource,
                            WhenDownloadedUtc = DateTime.UtcNow,
                            WhenLastAccessedUtc = DateTime.UtcNow
                        };
                    _entriesByHttpUrl[httpSource] = diskEntry;
                    _indexNeedsSaving = true;

                    var toCallback = _currentlyRequested[httpSource];
                    _currentlyRequested.Remove(httpSource);

                    foreach (var callbackPair in toCallback)
                    {
                        DoFilePathCallback(diskEntry, callbackPair.Success, callbackPair.Error);
                    }
                });
        }

        private void OnDownloadError(string httpSource, Exception exception)
        {
            List<CallbackPair> toCallback = null;
            RunSyncOrAsyncWithLock( () =>
                            {
                                toCallback = _currentlyRequested[httpSource];
                                _currentlyRequested.Remove(httpSource);
                            },
                          () =>
                              {
                                  foreach (var callbackPair in toCallback)
                                  {
                                      callbackPair.Error(exception);
                                  }
                              });
        }

        private void DoFilePathCallback(Entry diskEntry, Action<string> success, Action<Exception> error)
        {
            success(diskEntry.DownloadedPath);
        }

        public delegate void TimerCallback(object state);

        public sealed class Timer : CancellationTokenSource, IDisposable
        {
            public Timer(TimerCallback callback, object state, TimeSpan dueTime, TimeSpan period)
            {
                Task.Delay(dueTime, Token).ContinueWith(async (t, s) =>
                {
                    var tuple = (Tuple<TimerCallback, object>)s;

                    while (true)
                    {
                        if (IsCancellationRequested)
                            break;
                        Task.Run(() => tuple.Item1(tuple.Item2));
                        await Task.Delay(period);
                    }

                }, Tuple.Create(callback, state), CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
            }

            public new void Dispose() { base.Cancel(); }
        }
    }
}
