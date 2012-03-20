#region Copyright
// <copyright file="MvxHttpFileDownloader.cs" company="Cirrious">
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
using Cirrious.MvvmCross.Interfaces.Platform.Images;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxHttpFileDownloader : IMvxHttpFileDownloader
    {
        private readonly Dictionary<MvxFileDownloadRequest, bool> _currentRequests = new Dictionary<MvxFileDownloadRequest, bool>();

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
                                              error(args.Exception);
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