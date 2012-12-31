#region Copyright

// <copyright file="MvxEnumJsonConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class MvxEnumJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
#if NETFX_CORE
            return objectType.GetTypeInfo().IsEnum;
#else
            return objectType.IsEnum;
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