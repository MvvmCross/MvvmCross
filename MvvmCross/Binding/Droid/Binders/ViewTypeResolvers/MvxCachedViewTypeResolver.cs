// MvxCachedViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public class MvxCachedViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private readonly IMvxViewTypeResolver _resolver;

        public MvxCachedViewTypeResolver(IMvxViewTypeResolver resolver)
        {
            _resolver = resolver;
        }

        public Type Resolve(string tagName)
        {
            Type toReturn;
            if (_cache.TryGetValue(tagName, out toReturn))
                return toReturn;

            toReturn = _resolver.Resolve(tagName);
            _cache[tagName] = toReturn;
            return toReturn;
        }
    }
}