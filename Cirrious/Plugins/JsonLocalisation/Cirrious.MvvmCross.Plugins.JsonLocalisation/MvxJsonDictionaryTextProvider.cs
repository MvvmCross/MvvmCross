#region Copyright
// <copyright file="MvxJsonDictionaryTextProvider.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Platform;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Plugins.Json;

namespace Cirrious.MvvmCross.Plugins.JsonLocalisation
{
    public abstract class MvxJsonDictionaryTextProvider 
        : MvxDictionaryBaseTextProvider
        , IMvxJsonDictionaryTextLoader
        , IMvxServiceConsumer
    {
        protected MvxJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {            
        }

        private IMvxJsonConverter JsonConvert
        {
            get
            {
                return this.GetService<IMvxJsonConverter>();
            }
        }

        #region IMvxJsonDictionaryTextLoader Members

        public abstract void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

        public void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson)
        {
            var entries  = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson);
            foreach (var kvp in entries)
            {
                AddOrReplace(namespaceKey, typeKey, kvp.Key, kvp.Value);
            }
        }

        #endregion
    }
}
