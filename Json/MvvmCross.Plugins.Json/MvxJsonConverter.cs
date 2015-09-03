// MvxJsonConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Platform;
using Newtonsoft.Json;

namespace MvvmCross.Plugins.Json
{
    public class MvxJsonConverter : IMvxJsonConverter
    {
        private static readonly JsonSerializerSettings Settings;

        static MvxJsonConverter()
        {
            Settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize,
                    Converters = new List<JsonConverter>
                        {
                            new MvxEnumJsonConverter(),
                        },
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                };
        }

        public T DeserializeObject<T>(string inputText)
        {
            return JsonConvert.DeserializeObject<T>(inputText, Settings);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise, Formatting.None, Settings);
        }

        public object DeserializeObject(Type type, string inputText)
        {
            return JsonConvert.DeserializeObject(inputText, type, Settings);
        }
    }
}