// MvxStringDictionaryTextSerializer.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Parse.StringDictionary
{
    public class MvxStringDictionaryTextSerializer
        : IMvxTextSerializer
    {
        public T DeserializeObject<T>(string inputText)
        {
            return (T) DeserializeObject(typeof (T), inputText);
        }

        public string SerializeObject(object toSerialise)
        {
            if (!(toSerialise is MvxViewModelRequest))
                throw new MvxException("This serializer only knows about MvxViewModelRequest");


            return Serialize((MvxViewModelRequest) toSerialise);
        }

        public object DeserializeObject(Type type, string inputText)
        {
            CheckIsViewModelRequest(type);

            var stringDictionaryParser = new MvxStringDictionaryParser();
            var dictionary = stringDictionaryParser.Parse(inputText);
            var toReturn = new MvxViewModelRequest();
            var viewModelTypeName = SafeGetValue(dictionary, "Type");
            toReturn.ViewModelType = DeserializeViewModelType(viewModelTypeName);
            toReturn.RequestedBy = new MvxRequestedBy
                {
                    Type = (MvxRequestedByType) int.Parse(SafeGetValue(dictionary, "By")),
                    AdditionalInfo = SafeGetValue(dictionary, "Info")
                };
            toReturn.ParameterValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Params"));
            toReturn.PresentationValues = stringDictionaryParser.Parse(SafeGetValue(dictionary, "Pres"));
            return toReturn;
        }

        private static void CheckIsViewModelRequest(Type toCheck)
        {
            if (toCheck != typeof (MvxViewModelRequest))
                throw new MvxException("This serializer only knows about MvxViewModelRequest");
        }

        protected virtual string Serialize(MvxViewModelRequest toSerialise)
        {
            var stringDictionaryWriter = new MvxStringDictionaryWriter();

            var dictionary = new Dictionary<string, string>();
            dictionary["Type"] = SerializeViewModelName(toSerialise.ViewModelType);
            var requestedBy = toSerialise.RequestedBy ?? new MvxRequestedBy();
            dictionary["By"] = ((int) requestedBy.Type).ToString();
            dictionary["Info"] = requestedBy.AdditionalInfo;
            dictionary["Params"] = stringDictionaryWriter.Write(toSerialise.ParameterValues);
            dictionary["Pres"] = stringDictionaryWriter.Write(toSerialise.PresentationValues);
            return stringDictionaryWriter.Write(dictionary);
        }

        protected virtual string SerializeViewModelName(Type viewModelType)
        {
            return viewModelType.AssemblyQualifiedName;
        }

        protected virtual Type DeserializeViewModelType(string viewModelTypeName)
        {
            return Type.GetType(viewModelTypeName);
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