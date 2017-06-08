// MvxDictionaryTextProvider.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Platform.Platform;
using System.Collections.Generic;

namespace MvvmCross.Plugins.JsonLocalization
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

        #region Implementation of IMvxTextProvider

        public override string GetText(string namespaceKey, string typeKey, string name)
        {
            var key = MakeLookupKey(namespaceKey, typeKey, name);
            string value;
            if (_entries.TryGetValue(key, out value))
                return value;

            MvxTrace.Trace("Text value missing for " + key);
            if (_maskErrors)
                return key;

            throw new KeyNotFoundException("Could not find text lookup for " + key);
        }

        #endregion Implementation of IMvxTextProvider
    }
}