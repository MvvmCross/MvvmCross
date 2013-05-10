using System;
using System.IO;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;

namespace Cirrious.MvvmCross.Plugins.Network.Rest
{
    public class MvxJsonRestClient
        : MvxRestClient
          , IMvxJsonRestClient
    {
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public void MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction, Action<Exception> errorAction)
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
}