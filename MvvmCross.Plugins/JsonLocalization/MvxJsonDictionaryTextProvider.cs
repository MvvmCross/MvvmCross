// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Base;

namespace MvvmCross.Plugin.JsonLocalization
{
    public abstract class MvxJsonDictionaryTextProvider
        : MvxDictionaryTextProvider
         , IMvxJsonDictionaryTextLoader
    {
        protected MvxJsonDictionaryTextProvider(bool maskErrors)
            : base(maskErrors)
        {
        }

        private IMvxJsonConverter _jsonConvert;
        protected IMvxJsonConverter JsonConvert
        {
            get
            {
                _jsonConvert = _jsonConvert ?? Mvx.IoCProvider.Resolve<IMvxJsonConverter>();
                return _jsonConvert;
            }
        }

        [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
        public abstract void LoadJsonFromResource(string namespaceKey, string typeKey, string resourcePath);

        [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
        public virtual void LoadJsonFromText(string namespaceKey, string typeKey, string rawJson)
        {
            var entries = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawJson);
            foreach (var kvp in entries)
            {
                AddOrReplace(namespaceKey, typeKey, kvp.Key, kvp.Value);
            }
        }
    }
}
