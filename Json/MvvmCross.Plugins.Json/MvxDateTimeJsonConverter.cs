// MvxDateTimeJsonConverter.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Newtonsoft.Json;
using System;
using System.Globalization;

namespace MvvmCross.Plugins.Json
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
            //MvxTrace.Trace("About to write {0}", text);
            writer.WriteValue(text);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
                                        JsonSerializer serializer)
        {
            var text = reader.Value.ToString();
            //MvxTrace.Trace("About to parse {0}", text);
            return DateTime.ParseExact(text, DateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}