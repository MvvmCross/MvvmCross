// MvxJsonFlattener.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Cirrious.MvvmCross.Plugins.Json
{
    public class MvxJsonFlattener : IMvxJsonFlattener
    {
        public bool IsJsonObject(object input)
        {
            return input is JObject;
        }

        public object FlattenJsonObjectToStringDictionary(object input)
        {
            var jInput = input as JObject;
            if (jInput == null)
            {
                return input;
            }

            return jInput.ToObject<Dictionary<string, string>>();
        }
    }
}