// MvxHttpFileDownloader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MvvmCross.Plugins.DownloadCache
{
    public class MvxHttpFileDownloader
        : MvxLockableObject
        , IMvxHttpFileDownloader
    {
        private readonly Dictionary<MvxFileDownloadRequest, bool> _currentRequests =
            new Dictionary<MvxFileDownloadRequest, bool>();

        private const int DefaultMaxConcurrentDownloads = 20;
        private readonly int _maxConcurrentDownloads;
        private readonly Queue<MvxFileDownloadRequest> _queuedRequests = new Queue<MvxFileDownloadRequest>();

        public MvxHttpFileDownloader(int maxConcurrentDownloads = DefaultMaxConcurrentDownloads)
        {
            _maxConcurrentDownloads = maxConcurrentDownloads;
        }

        #region IMvxHttpFileDownloader Members

        public void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error)
        {
            var request = new MvxFileDownloadRequest(url, downloadPath);
            request.DownloadComplete += (sender, args) =>
            {
                OnRequestFinished(request);
                success?.Invoke();
            };
            request.DownloadFailed += (sender, args) =>
            {
                OnRequestFinished(request);
                error?.Invoke(args.Value);
            };

            RunSyncOrAsyncWithLock(() =>
            {
                _queuedRequests.Enqueue(request);
                if (_currentRequests.Count < _maxConcurrentDownloads)
                {
                    StartNextQueuedItem();
                }
            });
        }

        #endregion IMvxHttpFileDownloader Members

        private void OnRequestFinished(MvxFileDownloadRequest request)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                _currentRequests.Remove(request);
                if (_queuedRequests.Any())
                {
                    StartNextQueuedItem();
                }
            });
        }

        private void StartNextQueuedItem()
        {
            if (_currentRequests.Count >= _maxConcurrentDownloads)
                return;

            RunSyncOrAsyncWithLock(() =>
            {
                if (_currentRequests.Count >= _maxConcurrentDownloads)
                    return;

                if (!_queuedRequests.Any())
                    return;

                var request = _queuedRequests.Dequeue();
                _currentRequests.Add(request, true);
                request.Start();
            });
        }
    }
}