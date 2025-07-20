// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using MvvmCross.Base;

namespace MvvmCross.Plugin.Json
{
    [Preserve(AllMembers = true)]
    [RequiresUnreferencedCode("Uses JsonSerializer which may not be fully preserved in trimming scenarios")]
    public class MvxJsonConverter
        : IMvxJsonConverter
    {
        public JsonSerializerOptions Settings { get; set; }

        public MvxJsonConverter()
        {
            Settings = new JsonSerializerOptions
            {
                WriteIndented = false
            };
        }

        public T? DeserializeObject<T>(string inputText)
        {
            return JsonSerializer.Deserialize<T>(inputText, Settings);
        }

        public object? DeserializeObject(Type type, string inputText)
        {
            return JsonSerializer.Deserialize(inputText, type, Settings);
        }

        public T? DeserializeObject<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, Settings);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonSerializer.Serialize(toSerialise, Settings);
        }
    }
}
