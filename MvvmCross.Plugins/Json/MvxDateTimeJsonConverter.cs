// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace MvvmCross.Plugin.Json
{
    [Preserve(AllMembers = true)]
    public class MvxDateTimeJsonConverter : JsonConverter
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = ((DateTime)value).ToString(DateTimeFormat, CultureInfo.InvariantCulture);
            //MvxPluginLog.Instance.Trace("About to write {0}", text);
            writer.WriteValue(text);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var text = reader.Value.ToString();
            //MvxPluginLog.Instance.Trace("About to parse {0}", text);
            return DateTime.ParseExact(text, DateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}
