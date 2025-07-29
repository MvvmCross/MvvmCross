// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.ViewModels;

namespace MvvmCross.Core.Parse.StringDictionary;

public class MvxViewModelRequestCustomTextSerializer
    : IMvxTextSerializer
{
    protected Lazy<IMvxViewModelByNameLookup?> ByNameLookup { get; } =
        new(() => Mvx.IoCProvider?.Resolve<IMvxViewModelByNameLookup>());

    private readonly Lazy<MvxStringDictionaryWriter> _stringDictionaryWriter =
        new(() => new MvxStringDictionaryWriter());

    private readonly Lazy<MvxStringDictionaryParser> _stringDictionaryParser =
        new(() => new MvxStringDictionaryParser());

    public string SerializeObject(object toSerialise)
    {
        if (toSerialise is MvxViewModelRequest viewModelRequest)
            return Serialize(viewModelRequest);

        if (toSerialise is IDictionary<string, string> stringDictionary)
            return Serialize(stringDictionary);

        throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    public T DeserializeObject<T>(string inputText)
    {
        return (T)DeserializeObject(typeof(T), inputText);
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    public object DeserializeObject(Type type, string inputText)
    {
        if (type == typeof(MvxViewModelRequest))
            return DeserializeViewModelRequest(inputText);

        if (typeof(IDictionary<string, string>).IsAssignableFrom(type))
            return DeserializeStringDictionary(inputText);

        throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
    }

    protected virtual IDictionary<string, string> DeserializeStringDictionary(string inputText)
    {
        var dictionary = _stringDictionaryParser.Value.Parse(inputText);
        return dictionary;
    }

    [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
    protected virtual MvxViewModelRequest DeserializeViewModelRequest(string inputText)
    {
        var dictionary = _stringDictionaryParser.Value.Parse(inputText);
        var toReturn = new MvxViewModelRequest();
        var viewModelTypeName = SafeGetValue(dictionary, "Type");
        toReturn.ViewModelType = DeserializeViewModelType(viewModelTypeName);
        toReturn.ParameterValues = _stringDictionaryParser.Value.Parse(SafeGetValue(dictionary, "Params"));
        toReturn.PresentationValues = _stringDictionaryParser.Value.Parse(SafeGetValue(dictionary, "Pres"));
        return toReturn;
    }

    protected virtual string Serialize(IDictionary<string, string> toSerialise)
    {
        return _stringDictionaryWriter.Value.Write(toSerialise);
    }

    protected virtual string Serialize(MvxViewModelRequest toSerialise)
    {
        if (toSerialise == null)
            throw new ArgumentNullException(nameof(toSerialise));

        var dictionary = new Dictionary<string, string>
        {
            ["Type"] = SerializeViewModelName(toSerialise.ViewModelType),
            ["Params"] = _stringDictionaryWriter.Value.Write(toSerialise.ParameterValues),
            ["Pres"] = _stringDictionaryWriter.Value.Write(toSerialise.PresentationValues)
        };
        return _stringDictionaryWriter.Value.Write(dictionary);
    }

    protected virtual string SerializeViewModelName(Type? viewModelType)
    {
        if (viewModelType?.FullName == null)
            throw new ArgumentNullException(nameof(viewModelType));

        return viewModelType.FullName;
    }

    protected virtual Type? DeserializeViewModelType(string viewModelTypeName)
    {
        if (ByNameLookup.Value?.TryLookupByFullName(viewModelTypeName, out var toReturn) != true)
        {
            throw new MvxException(
                "Failed to find viewmodel for {0} - is the ViewModel in the same Assembly as App.cs? If not, you can add it by overriding GetViewModelAssemblies() in setup",
                viewModelTypeName);
        }

        return toReturn;
    }

    private static string SafeGetValue(IDictionary<string, string> dictionary, string key)
    {
        if (!dictionary.TryGetValue(key, out var value))
            throw new MvxException("Dictionary missing required key/value pair for key {0}", key);
        return value;
    }
}
