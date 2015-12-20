// MvxCachedViewTypeResolver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    using System;
    using System.Collections.Generic;

    public class MvxCachedViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private readonly IMvxViewTypeResolver _resolver;

        public MvxCachedViewTypeResolver(IMvxViewTypeResolver resolver)
        {
            this._resolver = resolver;
        }

        public Type Resolve(string tagName)
        {
            Type toReturn;
            if (this._cache.TryGetValue(tagName, out toReturn))
                return toReturn;

            toReturn = this._resolver.Resolve(tagName);
            this._cache[tagName] = toReturn;
            return toReturn;
        }
    }
}