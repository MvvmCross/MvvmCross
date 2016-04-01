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
using System.Threading.Tasks;

namespace MvvmCross.Plugins.Network.Rest
{
    public class MvxJsonRestClient
        : MvxRestClient
          , IMvxJsonRestClient
    {
        public Func<IMvxJsonConverter> JsonConverterProvider { get; set; }

        public async Task<MvxDecodedRestResponse<T>> MakeRequestFor<T>(MvxRestRequest restRequest)
        {
            var streamResponse = await MakeStreamRequest(restRequest);

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
                return decodedResponse;
            }
        }

        public MvxJsonRestClient()
        {
            JsonConverterProvider = () => Mvx.Resolve<IMvxJsonConverter>();
        }
    }
}