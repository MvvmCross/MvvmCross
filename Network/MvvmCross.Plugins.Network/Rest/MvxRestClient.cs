// MvxRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxRestClient : IMvxRestClient
    {
        protected Dictionary<string, object> Options { set; private get; }

        public MvxRestClient()
        {
            Options = new Dictionary<string, object>
                {
                    {MvxKnownOptions.ForceWindowsPhoneToUseCompression, "true"}
                };
        }

        public void ClearSetting(string key)
        {
            try
            {
                Options.Remove(key);
            }
            catch (KeyNotFoundException)
            {
                // ignored - not a problem
            }
        }

        public void SetSetting(string key, object value)
        {
            Options[key] = value;
        }

        public Task<MvxStreamRestResponse> MakeStreamRequestAsync(MvxRestRequest restRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            TaskCompletionSource<MvxStreamRestResponse> taskCompletionSource = new TaskCompletionSource<MvxStreamRestResponse>();

            HttpWebRequest httpRequest = BuildHttpRequest(restRequest);

            using (CancellationTokenRegistration tokenRegistration = cancellationToken.Register(() =>
            {
                httpRequest.Abort();
                taskCompletionSource.SetCanceled();
            }))
            {
                Action processResponse = () => ProcessStreamResponse(restRequest, httpRequest, response => taskCompletionSource.SetResult(response));
                if (restRequest.NeedsRequestStream)
                {
                    ProcessRequestThen(restRequest, httpRequest, processResponse);
                }
                else
                {
                    processResponse();
                }
            }

            return taskCompletionSource.Task;
        }

        public Task<MvxRestResponse> MakeRequestAsync(MvxRestRequest restRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            TaskCompletionSource<MvxRestResponse> taskCompletionSource = new TaskCompletionSource<MvxRestResponse>();

            HttpWebRequest httpRequest = BuildHttpRequest(restRequest);

            using (CancellationTokenRegistration tokenRegistration = cancellationToken.Register(() =>
            {
                httpRequest.Abort();
                taskCompletionSource.SetCanceled();
            }))
            {
                Action processResponse = () => ProcessResponse(restRequest, httpRequest, response => taskCompletionSource.SetResult(response));
                if (restRequest.NeedsRequestStream)
                {
                    ProcessRequestThen(restRequest, httpRequest, processResponse);
                }
                else
                {
                    processResponse();
                }
            }

            return taskCompletionSource.Task;
        }

        protected virtual HttpWebRequest BuildHttpRequest(MvxRestRequest restRequest)
        {
            var httpRequest = CreateHttpWebRequest(restRequest);
            SetMethod(restRequest, httpRequest);
            SetContentType(restRequest, httpRequest);
            SetUserAgent(restRequest, httpRequest);
            SetAccept(restRequest, httpRequest);
            SetCookieContainer(restRequest, httpRequest);
            SetCredentials(restRequest, httpRequest);
            SetCustomHeaders(restRequest, httpRequest);
            SetPlatformSpecificProperties(restRequest, httpRequest);
            return httpRequest;
        }

        private static void SetCustomHeaders(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            if (restRequest.Headers != null)
            {
                foreach (var kvp in restRequest.Headers)
                {
                    httpRequest.Headers[kvp.Key] = kvp.Value;
                }
            }
        }

        protected virtual void SetCredentials(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            if (restRequest.Credentials != null)
            {
                httpRequest.Credentials = restRequest.Credentials;
            }
        }

        protected virtual void SetCookieContainer(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            // note that we don't call
            //   httpRequest.SupportsCookieContainer
            // here - this is because Android complained about this...
            try
            {
                if (restRequest.CookieContainer != null)
                {
                    httpRequest.CookieContainer = restRequest.CookieContainer;
                }
            }
            catch (Exception exception)
            {
                Mvx.Warning("Error masked during Rest call - cookie creation - {0}", exception.ToLongString());
            }
        }

        protected virtual void SetAccept(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            if (!string.IsNullOrEmpty(restRequest.Accept))
            {
                httpRequest.Accept = restRequest.Accept;
            }
        }

        protected virtual void SetUserAgent(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            if (!string.IsNullOrEmpty(restRequest.UserAgent))
            {
                httpRequest.Headers["user-agent"] = restRequest.UserAgent;
            }
        }

        protected virtual void SetContentType(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            if (!string.IsNullOrEmpty(restRequest.ContentType))
            {
                httpRequest.ContentType = restRequest.ContentType;
            }
        }

        protected virtual void SetMethod(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            httpRequest.Method = restRequest.Verb;
        }

        protected virtual HttpWebRequest CreateHttpWebRequest(MvxRestRequest restRequest)
        {
            return (HttpWebRequest)WebRequest.Create(restRequest.Uri);
        }

        protected virtual void SetPlatformSpecificProperties(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            // do nothing by default
        }

        protected virtual void ProcessResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest,Action<MvxRestResponse> successAction)
        {
            httpRequest.BeginGetResponse(result =>
            {
                var response = (HttpWebResponse)httpRequest.EndGetResponse(result);

                var code = response.StatusCode;

                var restResponse = new MvxRestResponse
                {
                    CookieCollection = response.Cookies,
                    Tag = restRequest.Tag,
                    StatusCode = code
                };
                successAction?.Invoke(restResponse);
            }, null);
        }

        protected virtual void ProcessStreamResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest, Action<MvxStreamRestResponse> successAction)
        {
            httpRequest.BeginGetResponse(result =>
            {
                var response = (HttpWebResponse)httpRequest.EndGetResponse(result);

                var code = response.StatusCode;
                var responseStream = response.GetResponseStream();
                var restResponse = new MvxStreamRestResponse
                {
                    CookieCollection = response.Cookies,
                    Stream = responseStream,
                    Tag = restRequest.Tag,
                    StatusCode = code
                };
                successAction?.Invoke(restResponse);
            }, null);
        }

        protected virtual void ProcessRequestThen(MvxRestRequest restRequest, HttpWebRequest httpRequest, Action continueAction)
        {
            httpRequest.BeginGetRequestStream(result =>
            {
                using (var stream = httpRequest.EndGetRequestStream(result))
                {
                    restRequest.ProcessRequestStream(stream);
                    stream.Flush();
                }

                continueAction?.Invoke();
            }, null);
        }
    }
}