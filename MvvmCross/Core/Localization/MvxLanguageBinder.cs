// MvxLanguageBinder.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Localization
{
    using System;

    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;

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
            this._namespaceName = namespaceName;
            this._typeName = typeName;
        }

        private IMvxTextProvider _cachedTextProvider;

        protected virtual IMvxTextProvider TextProvider
        {
            get
            {
                if (this._cachedTextProvider != null)
                    return this._cachedTextProvider;

                lock (this)
                {
                    Mvx.TryResolve(out this._cachedTextProvider);
                    if (this._cachedTextProvider == null)
                    {
                        throw new MvxException(
                            "Missing text provider - please initialize IoC with a suitable IMvxTextProvider");
                    }
                    return this._cachedTextProvider;
                }
            }
        }

        public virtual string GetText(string entryKey)
        {
            return this.GetText(this._namespaceName, this._typeName, entryKey);
        }

        public virtual string GetText(string entryKey, params object[] args)
        {
            var format = this.GetText(entryKey);
            return string.Format(format, args);
        }

        protected virtual string GetText(string namespaceKey, string typeKey, string entryKey)
        {
            return this.TextProvider.GetText(namespaceKey, typeKey, entryKey);
        }
    }
}