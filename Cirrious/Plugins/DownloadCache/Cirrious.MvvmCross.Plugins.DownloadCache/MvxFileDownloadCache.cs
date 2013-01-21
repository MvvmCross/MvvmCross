// MvxFileDownloadCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

#if !NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.Platform.Diagnostics;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform.Diagnostics;
using Cirrious.MvvmCross.Plugins.File;

#warning See issue  https://github.com/slodge/MvvmCross/issues/69
/*
 * TODO:
 * 
 * 2012-09-14 07:57:10.762 BestSellersTouch[7134:b103] mvx: Warning:  11.89 Failed to save cache index ../Library/Caches/Pictures.MvvmCross/ - reason IOException: Sharing violation on path /Users/imac/Library/Application Support/iPhone Simulator/4.2/Applications/86CFE7E8-1CB9-4F3C-B574-2CE3217824A3/Library/Caches/Pictures.MvvmCross/_CacheIndex.txt
	  at System.IO.FileStream..ctor (System.String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, Boolean anonymous, FileOptions options) [0x00275] in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.IO/FileStream.cs:310 
  at System.IO.FileStream..ctor (System.String path, FileMode mode, FileAccess access, FileShare share) [0x00000] in <filename unknown>:0 
  at System.IO.File.OpenWrite (System.String path) [0x00000] in /Developer/MonoTouch/Source/mono/mcs/class/corlib/System.IO/File.cs:347 
  at Cirrious.MvvmCross.Plugins.File.Touch.MvxBaseFileStoreService.WriteFileCommon (System.String path, System.Action`1 streamAction) [0x00019] in /Users/imac/Documents/MvvmCrossNew/Cirrious/Plugins/File/Cirrious.MvvmCross.Plugins.File.Touch/MvxBaseFileStoreService.cs:173 
  at Cirrious.MvvmCross.Plugins.File.Touch.MvxBaseFileStoreService.WriteFile (System.String path, System.String contents) [0x0000d] in /Users/imac/Documents/MvvmCrossNew/Cirrious/Plugins/File/Cirrious.MvvmCross.Plugins.File.Touch/MvxBaseFileStoreService.cs:102 
  at Cirrious.MvvmCross.Plugins.DownloadCache.MvxFileDownloadCache.SaveIndexIfDirty () [0x00054] in /Users/imac/Documents/MvvmCrossNew/Cirrious/Plugins/DownloadCache/Cirrious.MvvmCross.Plugins.DownloadCache/MvxFileDownloadCache.cs:247 
*/

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxFileDownloadCache
        : IMvxFileDownloadCache
          , IMvxServiceConsumer
    {
        private const string CacheIndexFileName = "_CacheIndex.txt";
        private static readonly TimeSpan PeriodSaveInterval = TimeSpan.FromSeconds(1.0);

        private IMvxTextSerializer TextConvert
        {
            get { return this.GetService<IMvxTextSerializer>(); }
        }

#if MONOTOUCH
        [MonoTouch.Foundation.Preserve(AllMembers = true)]
#endif

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
                var fileService = this.GetService<IMvxSimpleFileStoreService>();
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
            var store = this.GetService<IMvxSimpleFileStoreService>();
            var files = store.GetFilesIn(_cacheFolder);

            // we don't use Linq because of AOT/JIT problem on MonoTouch :/
            //var cachedFiles = _entriesByHttpUrl.ToDictionary(x => x.Value.DownloadedPath);
            var cachedFiles = new Dictionary<string, Entry>();
            foreach (var e in _entriesByHttpUrl)
            {
                cachedFiles[e.Value.DownloadedPath] = e.Value;
            }

            var toDelete = new List<string>();
            foreach (var file in files)
            {
                if (!cachedFiles.ContainsKey(file))
                    if (!file.EndsWith(CacheIndexFileName))
                        toDelete.Add(file);
            }

            lock (this)
            {
                _toDeleteFiles.AddRange(toDelete);
            }
        }

        private void EnsureCacheFolderExists()
        {
            var store = this.GetService<IMvxSimpleFileStoreService>();
            store.EnsureFolderExists(_cacheFolder);
        }

        private Dictionary<string, Entry> LoadIndexEntries()
        {
            try
            {
                var store = this.GetService<IMvxSimpleFileStoreService>();
                string text;
                if (store.TryReadTextFile(IndexFilePath, out text))
                {
                    var list = TextConvert.DeserializeObject<List<Entry>>(text);
                    return list.ToDictionary(x => x.HttpSource, x => x);
                }
            }
                //catch (ThreadAbortException)
                //{
                //    throw;
                //}
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Failed to read cache index {0} - reason {1}", _cacheFolder,
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
            lock (this)
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
            }
        }

        private void DeleteNextUnneededFile()
        {
            string nextFileToDelete;
            lock (this)
            {
                nextFileToDelete = _toDeleteFiles.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(nextFileToDelete))
                return;

            try
            {
                var fileService = this.GetService<IMvxSimpleFileStoreService>();
                if (fileService.Exists(nextFileToDelete))
                    fileService.DeleteFile(nextFileToDelete);
            }
                //catch (ThreadAbortException)
                //{
                //    throw;
                //}
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Problem seen deleting file {0} problem {1}", nextFileToDelete,
                               exception.ToLongString());
            }
        }

        private void SaveIndexIfDirty()
        {
            if (!_indexNeedsSaving)
                return;

            List<Entry> toSave;
            lock (this)
            {
                toSave = _entriesByHttpUrl.Values.ToList();
                _indexNeedsSaving = false;
            }

            try
            {
                var store = this.GetService<IMvxSimpleFileStoreService>();
                var text = TextConvert.SerializeObject(toSave);
                store.WriteFile(IndexFilePath, text);
            }
                //catch (ThreadAbortException)
                //{
                //    throw;
                //}
            catch (Exception exception)
            {
                MvxTrace.Trace(MvxTraceLevel.Warning, "Failed to save cache index {0} - reason {1}", _cacheFolder,
                               exception.ToLongString());
            }
        }

        #endregion

        public void RequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error)
        {
            ThreadPool.QueueUserWorkItem(ignored => DoRequestLocalFilePath(httpSource, success, error));
        }

        private void DoRequestLocalFilePath(string httpSource, Action<string> success, Action<Exception> error)
        {
            lock (this)
            {
                Entry diskEntry;
                if (_entriesByHttpUrl.TryGetValue(httpSource, out diskEntry))
                {
                    var service = this.GetService<IMvxSimpleFileStoreService>();
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
                var downloader = this.GetService<IMvxHttpFileDownloader>();
                var fileService = this.GetService<IMvxSimpleFileStoreService>();
                var pathForDownload = fileService.PathCombine(_cacheFolder, Guid.NewGuid().ToString("N"));
                downloader.RequestDownload(httpSource, pathForDownload,
                                           () => OnDownloadSuccess(httpSource, pathForDownload),
                                           (exception) => OnDownloadError(httpSource, exception));
            }
        }

        private void OnDownloadSuccess(string httpSource, string pathForDownload)
        {
            lock (this)
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
            }
        }

        private void OnDownloadError(string httpSource, Exception exception)
        {
            List<CallbackPair> toCallback;
            lock (this)
            {
                toCallback = _currentlyRequested[httpSource];
                _currentlyRequested.Remove(httpSource);
            }

            foreach (var callbackPair in toCallback)
            {
                callbackPair.Error(exception);
            }
        }

        private void DoFilePathCallback(Entry diskEntry, Action<string> success, Action<Exception> error)
        {
            success(diskEntry.DownloadedPath);
        }
    }
}

#endif