// MvxHttpFileDownloader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Core;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxHttpFileDownloader : IMvxHttpFileDownloader
    {
        private readonly Dictionary<MvxFileDownloadRequest, bool> _currentRequests =
            new Dictionary<MvxFileDownloadRequest, bool>();

        private const int DefaultMaxConcurrentDownloads = 10;
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
                    success();
                };
            request.DownloadFailed += (sender, args) =>
                {
                    OnRequestFinished(request);
                    error(args.Value);
                };

            lock (this)
            {
                _queuedRequests.Enqueue(request);
                if (_currentRequests.Count < _maxConcurrentDownloads)
                {
                    MvxAsyncDispatcher.BeginAsync(StartNextQueuedItem);
                }
            }
        }

        #endregion

        private void OnRequestFinished(MvxFileDownloadRequest request)
        {
            lock (this)
            {
                _currentRequests.Remove(request);
                if (_queuedRequests.Any())
                {
                    MvxAsyncDispatcher.BeginAsync(StartNextQueuedItem);
                }
            }
        }

        private void StartNextQueuedItem()
        {
            if (_currentRequests.Count >= _maxConcurrentDownloads)
                return;

            lock (this)
            {
                if (_currentRequests.Count >= _maxConcurrentDownloads)
                    return;

                if (!_queuedRequests.Any())
                    return;

                var request = _queuedRequests.Dequeue();
                _currentRequests.Add(request, true);
                request.Start();
            }
        }
    }
}