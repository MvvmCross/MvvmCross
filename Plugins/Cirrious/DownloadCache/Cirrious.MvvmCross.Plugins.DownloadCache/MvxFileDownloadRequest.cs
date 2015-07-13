// MvxFileDownloadRequest.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Net;
using Cirrious.CrossCore.Core;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public class MvxFileDownloadRequest
    {
        public MvxFileDownloadRequest(string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;
        }

        public string DownloadPath { get; private set; }
        public string Url { get; private set; }

        public event EventHandler<MvxFileDownloadedEventArgs> DownloadComplete;
        public event EventHandler<MvxValueEventArgs<Exception>> DownloadFailed;

        public void Start()
        {
            try
            {
                var request = WebRequest.Create(new Uri(Url));
                request.BeginGetResponse((result) => ProcessResponse(request, result), null);
            }
//#if !NETFX_CORE
//            catch (ThreadAbortException)
//            {
//                throw;
//            }
//#endif
            catch (Exception e)
            {
                FireDownloadFailed(e);
            }
        }

        private void ProcessResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                var fileService = MvxFileStoreHelper.SafeGetFileStore();
                var tempFilePath = DownloadPath + ".tmp";

                using (var resp = request.EndGetResponse(result))
                {
                    using (var s = resp.GetResponseStream())
                    {
                        fileService.WriteFile(tempFilePath,
                                              fileStream =>
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
//#if !NETFX_CORE
//            catch (ThreadAbortException)
//            {
//                throw;
//            }
//#endif
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
                handler(this, new MvxValueEventArgs<Exception>(exception));
        }

        private void FireDownloadComplete()
        {
            var handler = DownloadComplete;
            if (handler != null)
                handler(this, new MvxFileDownloadedEventArgs(Url, DownloadPath));
        }
    }
}