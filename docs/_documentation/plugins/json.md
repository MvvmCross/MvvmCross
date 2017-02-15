---
layout: documentation
title: Json
category: Plugins
---
The `Json` plugin provides a wrapper for a PCL version of the NewtonSoft Json.Net library allowing it to support.
[block:code]
{
  "codes": [
    {
      "code": "public interface IMvxJsonConverter \n  : IMvxTextSerializer\n  {\n  }\n\npublic interface IMvxTextSerializer\n{\n  T DeserializeObject<T>(string inputText);\n  string SerializeObject(object toSerialise);\n  object DeserializeObject(Type type, string inputText);\n}",
      "language": "csharp"
    }
  ]
}
[/block]
The Serialize and Deserialize implementations used are 'default' Json.Net implementations with `Formatting.None` specified along with the settings:
[block:code]
{
  "codes": [
    {
      "code": "new JsonSerializerSettings\n{\n  Converters = new List<JsonConverter>\n  {\n    new MvxEnumJsonConverter(),\n  },\n  DateFormatHandling = DateFormatHandling.IsoDateFormat,\n}",
      "language": "csharp"
    }
  ]
}
[/block]
The version of Json.Net referenced is an old PCL version - 4.5.4.14825

Json.Net is a popular library and it may be that the use of this old PCL version will conflict with another library your app is using. If this is the case, then it should be relatively straight-forward to **not** load the Json plugin, and to instead build your own implementations of `IMvxTextSerializer` and `IMvxJsonConverter`.

The Json plugin can optionally be requested **not** to register Json as `IMvxTextSerializer` using the configuration in Setup.cs:
[block:code]
{
  "codes": [
    {
      "code": "protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)\n{\n  if (plugin == typeof(Cirrious.MvvmCross.Plugins.Json.PluginLoader))\n  {\n    return new Cirrious.MvvmCross.Plugins.Json.MvxJsonConfiguration()\n    {\n      RegisterAsTextSerializer = false\n    };\n  }\n\n  return null;\n}",
      "language": "csharp"
    }
  ]
}
[/block]
The Json plugin can be used:
[block:code]
{
  "codes": [
    {
      "code": "public class ExampleObject\n{\n  public string Name { get; set; }\n  public DateTime DateOfBirth { get; set; }\n}\n\nvar serializer = Mvx.Resolve<IMvxJsonConverter>();\n\nvar exampleObject = new ExampleObject()\n{\n  Name = \"Fred bloggs\",\n  DateOfBirth = new DateTime(1972,7,13)\n};\n\nvar jsonText = serializer.SerializeObject(exampleObject);\n\nvar deserialized = serializer.DeserializeObject<ExampleObject>(jsonText);",
      "language": "csharp"
    }
  ]
}
[/block]
Note that other Json libraries are available for .Net - in particular, several people have recommended the ServiceStack.Text library which is available on many platforms.