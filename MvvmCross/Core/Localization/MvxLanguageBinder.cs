// MvxLanguageBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using System;

namespace Cirrious.MvvmCross.Localization
{
    public class MvxLanguageBinder
        : IMvxLanguageBinder
    {
        private readonly string _namespaceName;
        private readonly string _typeName;

        public MvxLanguageBinder(Type owningObject)
            : this(owningObject.Namespace, owningObject.Name)
        {
        }

        public MvxLanguageBinder(string namespaceName = null, string typeName = null)
        {
            _namespaceName = namespaceName;
            _typeName = typeName;
        }

        private IMvxTextProvider _cachedTextProvider;

        protected virtual IMvxTextProvider TextProvider
        {
            get
            {
                if (_cachedTextProvider != null)
                    return _cachedTextProvider;

                lock (this)
                {
                    Mvx.TryResolve(out _cachedTextProvider);
                    if (_cachedTextProvider == null)
                    {
                        throw new MvxException(
                            "Missing text provider - please initialize IoC with a suitable IMvxTextProvider");
                    }
                    return _cachedTextProvider;
                }
            }
        }

        public virtual string GetText(string entryKey)
        {
            return GetText(_namespaceName, _typeName, entryKey);
        }

        public virtual string GetText(string entryKey, params object[] args)
        {
            var format = GetText(entryKey);
            return string.Format(format, args);
        }

        protected virtual string GetText(string namespaceKey, string typeKey, string entryKey)
        {
            return TextProvider.GetText(namespaceKey, typeKey, entryKey);
        }
    }
}