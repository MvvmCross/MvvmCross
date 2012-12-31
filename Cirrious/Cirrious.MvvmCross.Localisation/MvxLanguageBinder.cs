#region Copyright

// <copyright file="MvxLanguageBinder.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
//  
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using System;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Localization.Interfaces;

namespace Cirrious.MvvmCross.Localization
{
    public class MvxLanguageBinder
        : IMvxLanguageBinder
          , IMvxServiceConsumer<IMvxTextProvider>
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