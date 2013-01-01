// MvxJsonConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class MvxJsonConverter : IMvxJsonConverter
    {
        private static readonly JsonSerializerSettings Settings;

        static MvxJsonConverter()
        {
            Settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>
                        {
                            new MvxEnumJsonConverter(),
                        },
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                };
        }

        #region Implementation of IMvxJsonConverter

        public T DeserializeObject<T>(string inputText)
        {
            return JsonConvert.DeserializeObject<T>(inputText, Settings);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise, Formatting.None, Settings);
        }

        #endregion
    }
}