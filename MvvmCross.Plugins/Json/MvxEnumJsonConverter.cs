// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Newtonsoft.Json;

namespace MvvmCross.Plugin.Json
{
    [Preserve(AllMembers = true)]
    public class MvxEnumJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
#if NETFX_CORE
            return objectType.GetTypeInfo().IsEnum;
#else
            return objectType.GetTypeInfo().IsEnum;
#endif
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var theString = reader.Value.ToString();
            return Enum.Parse(objectType, theString, false);
            //return Enum.ToObject(objectType, int.Parse());
        }
    }
}
