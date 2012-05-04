#region Copyright
// <copyright file="MvxDateTimeJsonConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Platform.Json
{
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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var text = reader.Value.ToString();
            //MvxTrace.Trace("About to parse {0}", text);
            return DateTime.ParseExact(text, DateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}