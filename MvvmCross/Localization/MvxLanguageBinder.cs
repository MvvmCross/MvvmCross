// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Exceptions;

namespace MvvmCross.Localization
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
                    Mvx.IoCProvider.TryResolve(out _cachedTextProvider);
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