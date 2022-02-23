// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
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
