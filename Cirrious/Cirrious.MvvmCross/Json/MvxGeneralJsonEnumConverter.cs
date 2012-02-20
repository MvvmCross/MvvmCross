using System;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Json
{
    public class MvxGeneralJsonEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override void WriteJson(JsonWriter writer, object
                                                              value, JsonSerializer serializer)
        {
            writer.WriteValue((int)value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enum.ToObject(objectType, int.Parse(reader.Value.ToString()));
        }
    }
}