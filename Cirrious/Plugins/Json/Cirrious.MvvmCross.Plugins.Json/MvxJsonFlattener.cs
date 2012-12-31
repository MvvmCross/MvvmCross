#region Copyright

// <copyright file="MvxJsonFlattener.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

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