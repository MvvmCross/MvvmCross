// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MvvmCross.Plugin.JsonLocalization
{
    [Preserve(AllMembers = true)]
    public class MvxDictionaryTextProvider : MvxTextProvider
    {
        private readonly Dictionary<string, string> _entries = new Dictionary<string, string>();
        private readonly bool _maskErrors;

        public MvxDictionaryTextProvider(bool maskErrors)
        {
            _maskErrors = maskErrors;
        }

        protected void AddOrReplace(string namespaceKey, string typeKey, string name, string value)
        {
            var key = MakeLookupKey(namespaceKey, typeKey, name);
            _entries[key] = value;
        }

        public override string GetText(string namespaceKey, string typeKey, string name)
        {
            var key = MakeLookupKey(namespaceKey, typeKey, name);
            string value;
            if (_entries.TryGetValue(key, out value))
                return value;

            MvxPluginLog.Instance?.Log(LogLevel.Trace, "Text value missing for " + key);
            if (_maskErrors)
                return key;

            throw new KeyNotFoundException("Could not find text lookup for " + key);
        }

        public override bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name)
        {
            var key = MakeLookupKey(namespaceKey, typeKey, name);

            if (_entries.TryGetValue(key, out textValue))
                return true;

            MvxPluginLog.Instance?.Log(LogLevel.Trace, "Text value missing for " + key);

            textValue = key;
            return false;
        }
    }
}
