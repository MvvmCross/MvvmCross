// IMvxReachability.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Network
{
    public class MvxRestRequest
    {
        public MvxRestRequest(string url, string verb = MvxVerbs.Get, string tag = null)
            : this(new Uri(url), verb, tag)
        {            
        }

        public MvxRestRequest(Uri uri, string verb = MvxVerbs.Get, string tag = null)
        {
            Uri = uri;
            Tag = tag;
            Verb = verb;
            Headers = new Dictionary<string, string>();
        }

        public string Tag { get; set; }
        public Uri Uri { get; set; }
        public string Verb { get; set; }
        public string ContentType { get; set; }
        public string UserAgent { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public Dictionary<string, object> Settings { get; set; }

        public virtual bool NeedsRequestStream { get { return false; } }

        public virtual void ProcessRequestStream(Stream stream)
        {
            // base class does nothing
        }
    }

    public abstract class MvxTextBasedRestRequest
        : MvxRestRequest
    {
        protected MvxTextBasedRestRequest(string url, string verb = MvxVerbs.Get, string tag = null) : base(url, verb, tag)
        {
        }

        protected MvxTextBasedRestRequest(Uri uri, string verb = MvxVerbs.Get, string tag = null) : base(uri, verb, tag)
        {
        }

        protected static void WriteTextToStream(Stream stream, string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }

    public class MvxStringRestRequest
        : MvxTextBasedRestRequest
    {
        public MvxStringRestRequest(string url, string body = null, string verb = MvxVerbs.Get, string tag = null) : base(url, verb, tag)
        {
            Body = body;
        }

        public MvxStringRestRequest(Uri uri, string body = null, string verb = MvxVerbs.Get, string tag = null)
            : base(uri, verb, tag)
        {
            Body = body;
        }

        public override bool NeedsRequestStream { get { return !string.IsNullOrEmpty(Body); } }
        public string Body { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            WriteTextToStream(stream, Body);
        }
    }

    public class MvxStreamRestRequest
        : MvxRestRequest
    {
        public MvxStreamRestRequest(string url, Action<Stream> streamAction = null,  string verb = MvxVerbs.Get, string tag = null) : base(url, verb, tag)
        {
            BodyHandler = streamAction;
        }

        public MvxStreamRestRequest(Uri uri, Action<Stream> streamAction = null, string verb = MvxVerbs.Get, string tag = null)
            : base(uri, verb, tag)
        {
            BodyHandler = streamAction;
        }

        public override bool NeedsRequestStream { get { return BodyHandler != null; } }
        public Action<Stream> BodyHandler { get; set; }

        public override void ProcessRequestStream(Stream stream)
        {
            BodyHandler(stream);
        }
    }

    public class MvxRestResponse
    {
        public string Tag { get; set; }
        public CookieCollection CookieCollection { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class MvxStreamRestResponse
        : MvxRestResponse
    {
        public Stream Stream { get; set; }
    }

    public class MvxDecodedRestResponse<T>
        : MvxRestResponse
    {
        public T Result { get; set; }
    }

    public interface IMvxRestClient
    {
        void ClearSetting(string key);
        void SetSetting(string key, object value);

        void MakeRequest(MvxRestRequest restRequest, Action<MvxRestResponse> successAction, Action<Exception> errorAction);
        void MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction, Action<Exception> errorAction);
    }

    public interface IMvxJsonRestClient
    {
        Func<IMvxJsonConverter> JsonConverterProvider { get; set; } 
        void MakeRequest<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction, Action<Exception> errorAction);
    }

    public static class MvxVerbs
    {
        public const string Get = "GET";
        public const string Put = "PUT";
        public const string Post = "POST";
        public const string Delete = "DELETE";
    }

    public static class MvxContentType
    {
        public const string Json = "application/json";
        public const string WwwForm = "application/x-www-form-urlencoded";
        //public const string Xml = "application/xml";
    }

    public static class MvxKnownSettings
    {
        public const string ForceWindowsPhoneToUseCompression = "ForceWindowsPhoneToUseCompression";
    }

    public class MvxJsonRestClient
        : MvxRestClient
        , IMvxJsonRestClient
    {
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public void MakeRequest<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction, Action<Exception> errorAction)
        {
            MakeRequest(restRequest, (MvxStreamRestResponse streamResponse) =>
                {
                    using (var textReader = new StreamReader(streamResponse.Stream))
                    {
                        var text = textReader.ReadToEnd();
                        var result = JsonConverterProvider().DeserializeObject<T>(text);
                        var decodedResponse = new MvxDecodedRestResponse<T>()
                            {
                                CookieCollection = streamResponse.CookieCollection,
                                Result = result,
                                StatusCode = streamResponse.StatusCode,
                                Tag = streamResponse.Tag
                            };
                        successAction(decodedResponse);
                    }
                }, errorAction);
        }

        public MvxJsonRestClient()
        {
            JsonConverterProvider = () => Mvx.Resolve<IMvxJsonConverter>();
        }
    }

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

        protected Dictionary<string, object> Settings { set; private get; }

        public MvxRestClient()
        {
            Settings = new Dictionary<string, object>()
                {
                    { MvxKnownSettings.ForceWindowsPhoneToUseCompression, "true" }
                };
        }

        public void ClearSetting(string key)
        {
            try
            {
                Settings.Remove(key);
            }
            catch (KeyNotFoundException)
            {
                // ignored - not a problem
            }
        }

        public void SetSetting(string key, object value)
        {
            Settings[key] = value;
        }

        public void MakeRequest(MvxRestRequest restRequest, Action<MvxStreamRestResponse> successAction, Action<Exception> errorAction)
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

        private HttpWebRequest BuildHttpRequest(MvxRestRequest restRequest)
        {
            var httpRequest = (HttpWebRequest) WebRequest.Create(restRequest.Uri);

            httpRequest.Method = restRequest.Verb;
            if (!string.IsNullOrEmpty(restRequest.ContentType))
            {
                httpRequest.ContentType = restRequest.ContentType;
            }
            if (!string.IsNullOrEmpty(restRequest.UserAgent))
            {
                httpRequest.Headers["user-agent"] = restRequest.UserAgent;
            }

            if (httpRequest.SupportsCookieContainer
                && restRequest.CookieContainer != null)
            {
                httpRequest.CookieContainer = restRequest.CookieContainer;
            }

            if (restRequest.Headers != null)
            {
                foreach (var kvp in restRequest.Headers)
                {
                    httpRequest.Headers[kvp.Key] = kvp.Value;
                }
            }

            PlatformSpecificFill(restRequest, httpRequest);
            return httpRequest;
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

                            var restResponse = new MvxRestResponse()
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
                                var restResponse = new MvxStreamRestResponse()
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

        protected virtual void PlatformSpecificFill(MvxRestRequest restRequest, HttpWebRequest httpRequest)
        {
            // do nothing by default
        }
    }
}