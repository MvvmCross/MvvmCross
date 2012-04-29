#region Copyright
// <copyright file="MvxJsonConverter.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Interfaces.Platform;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Platform.Json
{
    public class MvxJsonConverter : IMvxJsonConverter
    {
        private static readonly JsonSerializerSettings Settings;

        static MvxJsonConverter()
        {
            Settings = new JsonSerializerSettings()
                           {
                               Converters = new List<JsonConverter>()
                                                {
                                                    new MvxGeneralJsonEnumConverter()
                                                }                                                
                           };
        }
 
        #region Implementation of IMvxJsonConverter

        public T DeserializeObject<T>(string jsonText)
        {
            return JsonConvert.DeserializeObject<T>(jsonText, Settings);
        }

        public string SerializeObject(object toSerialise)
        {
            return JsonConvert.SerializeObject(toSerialise, Formatting.None, Settings);
        }

        #endregion
    }
}