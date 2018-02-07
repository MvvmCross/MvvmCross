// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;

namespace MvvmCross.Plugin.DownloadCache
{
    [Preserve(AllMembers = true)]
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
