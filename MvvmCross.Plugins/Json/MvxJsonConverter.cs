// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using MvvmCross.Base;
using Newtonsoft.Json;

namespace MvvmCross.Plugin.Json
{
    [Preserve(AllMembers = true)]
    public class MvxJsonConverter
        : IMvxJsonConverter
    {
        private static readonly JsonSerializerSettings Settings;

        static MvxJsonConverter()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Converters = new List<JsonConverter>
                {
                    new MvxEnumJsonConverter()
                },
                DateFormatHandling = DateFormatHandling.IsoDateFormat
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

        public T DeserializeObject<T>(Stream stream)
        {
            var serializer = JsonSerializer.Create(Settings);

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
