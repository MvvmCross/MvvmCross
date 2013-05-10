// MvxRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Net;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
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
                errorAction(exception);
            }
        }

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

        public void MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction,
                                Action<Exception> errorAction)
        {
            TryCatch(() =>
                {
                    var httpRequest = BuildHttpRequest(restRequest);

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
        }

        public void MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction,
                                Action<Exception> errorAction)
        {
            TryCatch(() =>
                {
                    var httpRequest = BuildHttpRequest(restRequest);

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
            if (httpRequest.SupportsCookieContainer
                && restRequest.CookieContainer != null)
            {
                httpRequest.CookieContainer = restRequest.CookieContainer;
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
            return (HttpWebRequest) WebRequest.Create(restRequest.Uri);
        }

        protected virtual void SetPlatformSpecificProperties(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            // do nothing by default
        }

        protected virtual void ProcessResponse(
            MvxRestRequest restRequest,
            HttpWebRequest httpRequest,
            Action<MvxRestResponse> successAction,
            Action<Exception> errorAction)
        {
            httpRequest.BeginGetResponse(result =>
                                         TryCatch(() =>
                                             {
                                                 var response = (HttpWebResponse) httpRequest.EndGetResponse(result);

                                                 var code = response.StatusCode;

                                                 var restResponse = new MvxRestResponse
                                                     {
                                                         CookieCollection = response.Cookies,
                                                         Tag = restRequest.Tag,
                                                         StatusCode = code
                                                     };
                                                 successAction(restResponse);
                                             }, errorAction)
                                         , null);
        }

        protected virtual void ProcessResponse(
            MvxRestRequest restRequest,
            HttpWebRequest httpRequest,
            Action<MvxStreamRestResponse> successAction,
            Action<Exception> errorAction)
        {
            httpRequest.BeginGetResponse(result =>
                                         TryCatch(() =>
                                             {
                                                 var response = (HttpWebResponse) httpRequest.EndGetResponse(result);

                                                 var code = response.StatusCode;

                                                 using (var responseStream = response.GetResponseStream())
                                                 {
                                                     var restResponse = new MvxStreamRestResponse
                                                         {
                                                             CookieCollection = response.Cookies,
                                                             Stream = responseStream,
                                                             Tag = restRequest.Tag,
                                                             StatusCode = code
                                                         };
                                                     successAction(restResponse);
                                                 }
                                             }, errorAction)
                                         , null);
        }

        protected virtual void ProcessRequestThen(
            MvxRestRequest restRequest,
            HttpWebRequest httpRequest,
            Action continueAction,
            Action<Exception> errorAction)
        {
            httpRequest.BeginGetRequestStream(result =>
                                              TryCatch(() =>
                                                  {
                                                      using (var stream = httpRequest.EndGetRequestStream(result))
                                                      {
                                                          restRequest.ProcessRequestStream(stream);
                                                          stream.Flush();
                                                      }

                                                      continueAction();
                                                  }, errorAction)
                                              , null);
        }
    }
}