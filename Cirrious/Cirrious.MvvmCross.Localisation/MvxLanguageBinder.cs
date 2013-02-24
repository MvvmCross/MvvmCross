﻿// MvxLanguageBinder.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Localization.Interfaces;

namespace Cirrious.MvvmCross.Localization
{
    public class MvxLanguageBinder
        : IMvxLanguageBinder
          , IMvxConsumer
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

        private IMvxTextProvider TextProvider
        {
            get
            {
                if (_cachedTextProvider != null)
                    return _cachedTextProvider;

                lock (this)
                {
                    this.TryGetService(out _cachedTextProvider);
                    if (_cachedTextProvider == null)
                    {
                        throw new MvxException(
                            "Missing text provider - please initialise IoC with a suitable IMvxTextProvider");
                    }
                    return _cachedTextProvider;
                }
            }
        }

        #region Implementation of IMvxLanguageBinder

        public string GetText(string entryKey)
        {
            return GetText(_namespaceName, _typeName, entryKey);
        }

        public string GetText(string entryKey, params object[] args)
        {
            var format = GetText(entryKey);
            return string.Format(format, args);
        }

        private string GetText(string namespaceKey, string typeKey, string entryKey)
        {
            return TextProvider.GetText(namespaceKey, typeKey, entryKey);
        }

        #endregion
    }
}