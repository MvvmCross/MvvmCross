// MvxJsonRestClient.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxJsonRestClient
        : MvxRestClient
          , IMvxJsonRestClient
    {
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public IMvxAbortable MakeRequestFor<T>(MvxRestRequest restRequest, Action<MvxDecodedRestResponse<T>> successAction, Action<Exception> errorAction)
        {
            return MakeRequest(restRequest, (MvxStreamRestResponse streamResponse) =>
            {
                using (var textReader = new StreamReader(streamResponse.Stream))
                {
                    var text = textReader.ReadToEnd();
                    var result = JsonConverterProvider().DeserializeObject<T>(text);
                    var decodedResponse = new MvxDecodedRestResponse<T>
                    {
                        CookieCollection = streamResponse.CookieCollection,
                        Result = result,
                        StatusCode = streamResponse.StatusCode,
                        Tag = streamResponse.Tag
                    };
                    successAction?.Invoke(decodedResponse);
                }
            }, errorAction);
        }

        public async Task<MvxDecodedRestResponse<T>> MakeRequestForAsync<T>(MvxRestRequest restRequest, CancellationToken cancellationToken = default(CancellationToken))
        {
            var decodedResponse = new MvxDecodedRestResponse<T>();

            try
            {
                var streamResponse = await MakeStreamRequestAsync(restRequest, cancellationToken).ConfigureAwait(false);

                if(streamResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    decodedResponse.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    using (var textReader = new StreamReader(streamResponse.Stream))
                    {
                        var text = textReader.ReadToEnd();
                        var result = JsonConverterProvider().DeserializeObject<T>(text);

                        decodedResponse.CookieCollection = streamResponse.CookieCollection;
                        decodedResponse.Result = result;
                        decodedResponse.StatusCode = streamResponse.StatusCode;
                        decodedResponse.Tag = streamResponse.Tag;
                    }
                }
            }
            catch
            {
                decodedResponse.StatusCode = HttpStatusCode.BadRequest;
            }

            return decodedResponse;
        }

        public MvxJsonRestClient()
        {
            JsonConverterProvider = () => Mvx.Resolve<IMvxJsonConverter>();
        }
    }
}
