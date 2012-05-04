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
        private const string DateTimeFormat = "o";

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.ParseExact(reader.Value.ToString(), DateTimeFormat, CultureInfo.InvariantCulture);
        }
    }
}