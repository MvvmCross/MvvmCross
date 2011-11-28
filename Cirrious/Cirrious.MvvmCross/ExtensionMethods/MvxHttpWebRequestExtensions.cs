#region Copyright
// <copyright file="MvxHttpWebRequestExtensions.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace Cirrious.MvvmCross.ExtensionMethods
{
    public static class MvxHttpWebRequestExtensions
    {
        private const int DefaultRequestTimeout = 5000;

        public static bool IsUiThread()
        {
            return Deployment.Current.CheckAccess();
        }

        public static HttpWebResponse GetResponse(this HttpWebRequest request)
        {
            if (IsUiThread())
                return null;

            HttpWebResponse response = null;

            var mre = new ManualResetEvent(false);
            Exception ex = null;

            ThreadPool.QueueUserWorkItem(obj => request.BeginGetResponse(ar =>
            {
                try
                {
                    request = (HttpWebRequest)ar.AsyncState != null ? (HttpWebRequest)ar.AsyncState : request;
                    response = (HttpWebResponse)request.EndGetResponse(ar);
                }
                catch (Exception e)
                {
                    ex = e;
                }
                finally
                {
                    mre.Set();
                }
            },
                null
            ));

            mre.WaitOne(5000);

            if (ex != null)
                // throw an error if we encountered one
                throw ex;

            // return whatever we got
            return response;
        }

        public static Stream GetRequestStream(this HttpWebRequest request)
        {
            if (IsUiThread())
                return null;

            var dataReady = new AutoResetEvent(false);
            Stream stream = null;
            var callback = new AsyncCallback(delegate(IAsyncResult asynchronousResult)
            {
                stream = (Stream)request.EndGetRequestStream(asynchronousResult);
                dataReady.Set();
            });

            request.BeginGetRequestStream(callback, request);
            if (!dataReady.WaitOne(DefaultRequestTimeout))
            {
                return null;
            }

            return stream;
        }
    }
}
