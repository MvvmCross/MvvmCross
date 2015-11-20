// MvxViewModelRequestCustomTextSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
    public class MvxViewModelRequestCustomTextSerializer
        : IMvxTextSerializer
    {
        private IMvxViewModelByNameLookup _byNameLookup;

        protected IMvxViewModelByNameLookup ByNameLookup
        {
            get
            {
                _byNameLookup = _byNameLookup ?? Mvx.Resolve<IMvxViewModelByNameLookup>();
                return _byNameLookup;
            }
        }

        public T DeserializeObject<T>(string inputText)
        {
            return (T)DeserializeObject(typeof(T), inputText);
        }

        public string SerializeObject(object toSerialise)
        {
            if (toSerialise is MvxViewModelRequest)
                return Serialize((MvxViewModelRequest)toSerialise);

            if (toSerialise is IDictionary<string, string>)
                return Serialize((IDictionary<string, string>)toSerialise);

            throw new MvxException("This serializer only knows about MvxViewModelRequest and IDictionary<string,string>");
        }

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
            var stringDictionaryParser = new MvxStringDictionaryParser();
            var dictionary = stringDictionaryParser.Parse(inputText);
            return dictionary;
        }

        protected virtual MvxViewModelRequest DeserializeViewModelRequest(string inputText)
        {
            var stringDictionaryParser = new MvxStringDictionaryParser();
            var dictionary = stringDictionaryParser.Parse(inputText);
            var toReturn = new MvxViewModelRequest();
            var viewModelTypeName = SafeGetValue(dictionary, "Type");
            toReturn.ViewModelType = DeserializeViewModelType(viewModelTypeName);
            toReturn.RequestedBy = new MvxRequestedBy
            {
                Type = (MvxRequestedByType)int.Parse(SafeGetValue(dictionary, "By")),
                AdditionalInfo = SafeGetValue(dictionary, "Info")
            };
            toReturn.ParameterValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Params"));
            toReturn.PresentationValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Pres"));
            return toReturn;
        }

        protected virtual string Serialize(IDictionary<string, string> toSerialise)
        {
            var stringDictionaryWriter = new MvxStringDictionaryWriter();
            return stringDictionaryWriter.Write(toSerialise);
        }

        protected virtual string Serialize(MvxViewModelRequest toSerialise)
        {
            var stringDictionaryWriter = new MvxStringDictionaryWriter();

            var dictionary = new Dictionary<string, string>();
            dictionary["Type"] = SerializeViewModelName(toSerialise.ViewModelType);
            var requestedBy = toSerialise.RequestedBy ?? new MvxRequestedBy();
            dictionary["By"] = ((int)requestedBy.Type).ToString();
            dictionary["Info"] = requestedBy.AdditionalInfo;
            dictionary["Params"] = stringDictionaryWriter.Write(toSerialise.ParameterValues);
            dictionary["Pres"] = stringDictionaryWriter.Write(toSerialise.PresentationValues);
            return stringDictionaryWriter.Write(dictionary);
        }

        protected virtual string SerializeViewModelName(Type viewModelType)
        {
            return viewModelType.FullName;
        }

        protected virtual Type DeserializeViewModelType(string viewModelTypeName)
        {
            Type toReturn;
            if (!ByNameLookup.TryLookupByFullName(viewModelTypeName, out toReturn))
                throw new MvxException("Failed to find viewmodel for {0} - is the ViewModel in the same Assembly as App.cs? If not, you can add it by overriding GetViewModelAssemblies() in setup", viewModelTypeName);
            return toReturn;
        }

        private string SafeGetValue(IDictionary<string, string> dictionary, string key)
        {
            string value;
            if (!dictionary.TryGetValue(key, out value))
                throw new MvxException("Dictionary missing required keyvalue pair for key {0}", key);
            return value;
        }
    }
}