// MvxJsonDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.MvvmCross.ExtensionMethods;
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
            get { return this.GetService<IMvxJsonConverter>(); }
        }

        #region IMvxJsonDictionaryTextLoader Members

        public abstract void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

        public void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson)
        {
            var entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson);
            foreach (var kvp in entries)
            {
                AddOrReplace(namespaceKey, typeKey, kvp.Key, kvp.Value);
            }
        }

        #endregion
    }
}