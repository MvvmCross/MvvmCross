// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Plugins.JsonLocalization
{
    public abstract class MvxJsonDictionaryTextProvider
        : MvxDictionaryTextProvider
         , IMvxJsonDictionaryTextLoader
    {
        protected MvxJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {
        }

        private IMvxJsonConverter JsonConvert => Mvx.Resolve<IMvxJsonConverter>();

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