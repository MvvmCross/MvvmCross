using Newtonsoft.Json;
using System;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class MvxGuidJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Guid).IsAssignableFrom(objectType);
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
            if (string.IsNullOrEmpty(theString))
                return Guid.Empty;

            return new Guid(theString);
        }
    }
}