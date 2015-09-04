### Json

The `Json` plugin provides a wrapper for a PCL version of the NewtonSoft Json.Net library allowing it to support:

    public interface IMvxJsonConverter 
        : IMvxTextSerializer
    {
    }

    public interface IMvxTextSerializer
    {
        T DeserializeObject<T>(string inputText);
        string SerializeObject(object toSerialise);
        object DeserializeObject(Type type, string inputText);
    }

The Serialize and Deserialize implementations used are 'default' Json.Net implementations with `Formatting.None` specified along with the settings:

    new JsonSerializerSettings
    {
        Converters = new List<JsonConverter>
            {
                new MvxEnumJsonConverter(),
            },
        DateFormatHandling = DateFormatHandling.IsoDateFormat,
    }
 
The version of Json.Net referenced is an old PCL version - 4.5.4.14825

Json.Net is a popular library and it may be that the use of this old PCL version will conflict with another library your app is using. If this is the case, then it should be relatively straight-forward to **not** load the Json plugin, and to instead build your own implementations of `IMvxTextSerializer` and `IMvxJsonConverter`.

The Json plugin can optionally be requested **not** to register Json as `IMvxTextSerializer` using the configuration in Setup.cs:


        protected override IMvxPluginConfiguration GetPluginConfiguration(Type plugin)
        {
            if (plugin == typeof(Cirrious.MvvmCross.Plugins.Json.PluginLoader))
            {
                return new Cirrious.MvvmCross.Plugins.Json.MvxJsonConfiguration()
                {
                    RegisterAsTextSerializer = false
                };
            }
            
            return null;
        }

The Json plugin can be used:

        public class ExampleObject
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        var serializer = Mvx.Resolve<IMvxJsonConverter>();
        
        var exampleObject = new ExampleObject()
        {
             Name = "Fred bloggs",
             DateOfBirth = new DateTime(1972,7,13)
        };
        
        var jsonText = serializer.SerializeObject(exampleObject);
        
        var deserialized = serializer.DeserializeObject<ExampleObject>(jsonText);
        
Note that other Json libraries are available for .Net - in particular, several people have recommended the ServiceStack.Text library which is available on many platforms.