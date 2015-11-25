// MvxEnumJsonConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Newtonsoft.Json;
using System;
using System.Reflection;

namespace MvvmCross.Plugins.Json
{
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

        public override void WriteJson(JsonWriter writer, object
                                                              value, JsonSerializer serializer)
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