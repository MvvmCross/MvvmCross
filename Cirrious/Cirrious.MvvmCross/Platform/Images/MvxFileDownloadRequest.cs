#region Copyright
// <copyright file="MvxFileDownloadRequest.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Net;
using System.Threading;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;

namespace Cirrious.MvvmCross.Platform.Images
{
    public class MvxFileDownloadRequest
        : IMvxServiceConsumer<IMvxSimpleFileStoreService>
    {
        public MvxFileDownloadRequest(string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;
        }

        public string DownloadPath { get; private set; }
        public string Url { get; private set; }

        public event EventHandler<MvxFileDownloadedEventArgs> DownloadComplete;
        public event EventHandler<MvxExceptionEventArgs> DownloadFailed;

        public void Start()
        {
            try
            {
                var request = WebRequest.Create(new Uri(Url));
                request.BeginGetResponse((result) => ProcessResponse(request, result), null);
            }
#if !NETFX_CORE
            catch (ThreadAbortException)
            {
                throw;
            }
#endif
            catch (Exception e)
            {
                FireDownloadFailed(e);
            }
        }

        private void ProcessResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                var fileService = this.GetService<IMvxSimpleFileStoreService>();
                var tempFilePath = DownloadPath + ".tmp";

                using (var resp = request.EndGetResponse(result))
                {
                    using (var s = resp.GetResponseStream())
                    {
                        fileService.WriteFile(tempFilePath,
                                              (fileStream) =>
                                                  {
                                                      var buffer = new byte[4*1024];
                                                      int count;
                                                      while ((count = s.Read(buffer, 0, buffer.Length)) > 0)
                                                      {
                                                          fileStream.Write(buffer, 0, count);
                                                      }
                                                  });
                    }
                }
                fileService.TryMove(tempFilePath, DownloadPath, true);
            }
#if !NETFX_CORE
            catch (ThreadAbortException)
            {
                throw;
            }
#endif
            catch (Exception exception)
            {
                FireDownloadFailed(exception);
                return;
            }

            FireDownloadComplete();
        }

        private void FireDownloadFailed(Exception exception)
        {
            var handler = DownloadFailed;
            if (handler != null)
                handler(this, new MvxExceptionEventArgs(exception));
        }

        private void FireDownloadComplete()
        {
            var handler = DownloadComplete;
            if (handler != null)
                handler(this, new MvxFileDownloadedEventArgs(Url, DownloadPath));
        }
    }
}