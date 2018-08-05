// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Logging;

namespace MvvmCross.Plugin.Network.Rest
{
    [Preserve(AllMembers = true)]
    public class MvxRestClient : IMvxRestClient
    {
        protected static void TryCatch(Action toTry, Action<Exception> errorAction)
        {
            try
            {
                toTry();
            }
            catch (Exception exception)
            {
                errorAction?.Invoke(exception);
            }
        }

        protected Dictionary<string, object> Options { private get; set; }

        public MvxRestClient()
        {
            Options = new Dictionary<string, object>
                {
                    {MvxKnownOptions.ForceWindowsPhoneToUseCompression, "true"}
                };
        }

        #region settings

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

        #endregion

        #region BuildHttpRequest methods

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
                MvxPluginLog.Instance.Warn("Error masked during Rest call - cookie creation - {0}", exception.ToLongString());
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

        #endregion

        #region IMvxAbortable legacy methods

        public IMvxAbortable MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction, Action<Exception> errorAction)
        {
            HttpWebRequest httpRequest = null;

            TryCatch(() =>
            {
                httpRequest = BuildHttpRequest(restRequest);

                Action processResponse = () => ProcessResponse(restRequest, httpRequest, successAction, errorAction);
                if (restRequest.NeedsRequestStream)
                {
                    ProcessRequestThen(restRequest, httpRequest, processResponse, errorAction);
                }
                else
                {
                    processResponse();
                }
            }, errorAction);

            return httpRequest != null ? new MvxRestRequestAsyncHandle(httpRequest) : null;
        }

        public IMvxAbortable MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction, Action<Exception> errorAction)
        {
            HttpWebRequest httpRequest = null;

            TryCatch(() =>
            {
                httpRequest = BuildHttpRequest(restRequest);

                Action processResponse = () => ProcessResponse(restRequest, httpRequest, successAction, errorAction);
                if (restRequest.NeedsRequestStream)
                {
                    ProcessRequestThen(restRequest, httpRequest, processResponse, errorAction);
                }
                else
                {
                    processResponse();
                }
            }, errorAction);

            return httpRequest != null ? new MvxRestRequestAsyncHandle(httpRequest) : null;
        }

        protected virtual void ProcessRequestThen(MvxRestRequest restRequest, HttpWebRequest httpRequest, Action continueAction, Action<Exception> errorAction)
        {
            httpRequest.BeginGetRequestStream(result =>
                TryCatch(() =>
                {
                    using (var stream = httpRequest.EndGetRequestStream(result))
                    {
                        restRequest.ProcessRequestStream(stream);
                        stream.Flush();
                    }

                    continueAction?.Invoke();
                }, errorAction), null);
        }

        protected virtual void ProcessResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest, Action<MvxRestResponse> successAction, Action<Exception> errorAction)
        {
            httpRequest.BeginGetResponse(result =>
                TryCatch(() =>
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
                }, errorAction), null);
        }

        protected virtual void ProcessResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest, Action<MvxStreamRestResponse> successAction, Action<Exception> errorAction)
        {
            httpRequest.BeginGetResponse(result =>
                TryCatch(() =>
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
                }, errorAction), null);
        }

        #endregion

        public async Task<MvxStreamRestResponse> MakeStreamRequestAsync(MvxRestRequest restRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpWebRequest httpRequest = BuildHttpRequest(restRequest);

            using (CancellationTokenRegistration tokenRegistration = cancellationToken.Register(() => httpRequest.Abort()))
            {
                if (restRequest.NeedsRequestStream)
                {
                    try
                    {
                        await ProcessRequestThen(restRequest, httpRequest).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        return new MvxStreamRestResponse
                        {
                            Tag = restRequest?.Tag,
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                var response = await ProcessStreamResponse(restRequest, httpRequest).ConfigureAwait(false);

                return response;
            }
        }

        public async Task<MvxRestResponse> MakeRequestAsync(MvxRestRequest restRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpWebRequest httpRequest = BuildHttpRequest(restRequest);

            using (CancellationTokenRegistration tokenRegistration = cancellationToken.Register(() => httpRequest.Abort()))
            {
                if (restRequest.NeedsRequestStream)
                {
                    try
                    {
                        await ProcessRequestThen(restRequest, httpRequest).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        return new MvxRestResponse
                        {
                            Tag = restRequest?.Tag,
                            StatusCode = HttpStatusCode.BadRequest
                        };
                    }
                }

                var response = await ProcessResponse(restRequest, httpRequest).ConfigureAwait(false);

                return response;
            }
        }

        protected virtual async Task<MvxRestResponse> ProcessResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            var response = (HttpWebResponse)await httpRequest.GetResponseAsync().ConfigureAwait(false);

            var restResponse = new MvxRestResponse
            {
                CookieCollection = response.Cookies,
                Tag = restRequest.Tag,
                StatusCode = response.StatusCode
            };

            return restResponse;
        }

        protected virtual async Task<MvxStreamRestResponse> ProcessStreamResponse(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            var response = (HttpWebResponse)await httpRequest.GetResponseAsync().ConfigureAwait(false);

            var code = response.StatusCode;
            var responseStream = response.GetResponseStream();
            var restResponse = new MvxStreamRestResponse
            {
                CookieCollection = response.Cookies,
                Stream = responseStream,
                Tag = restRequest.Tag,
                StatusCode = code
            };

            return restResponse;
        }
        
        protected virtual async Task ProcessRequestThen(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            using (var stream = await httpRequest.GetRequestStreamAsync().ConfigureAwait(false))
            {
                restRequest.ProcessRequestStream(stream);
                stream.Flush();
            }
        }
    }
}
