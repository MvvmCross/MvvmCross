// MvxJsonDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using System.Collections.Generic;

namespace MvvmCross.Plugins.JsonLocalisation
{
    public abstract class MvxJsonDictionaryTextProvider
        : MvxDictionaryTextProvider
         , IMvxJsonDictionaryTextLoader
    {
        protected MvxJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {
        }

        private IMvxJsonConverter JsonConvert
        {
            get { return Mvx.Resolve<IMvxJsonConverter>(); }
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

        #endregion IMvxJsonDictionaryTextLoader Members
    }
}