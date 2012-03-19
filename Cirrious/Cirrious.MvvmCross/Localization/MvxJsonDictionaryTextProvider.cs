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
#if !NETFX_CORE
using System.Collections.Generic;
using System.IO;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Localization;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Newtonsoft.Json;

namespace Cirrious.MvvmCross.Localization
{
    public class MvxJsonDictionaryTextProvider 
        : MvxDictionaryBaseTextProvider
        , IMvxJsonDictionaryTextLoader
        , IMvxServiceConsumer<IMvxResourceLoader>
    {
        public MvxJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {            
        }

        #region IMvxJsonDictionaryTextLoader Members

        public void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath)
        {
            var service = this.GetService<IMvxResourceLoader>();
            var json = service.GetTextResource(resourcePath);
            if (string.IsNullOrEmpty(json))
                throw new FileNotFoundException("Unable to find resource file " + resourcePath);
            LoadJsonFromText(namespaceKey, typeKey, json);
        }

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
#endif
