// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public class MvxCompositeViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly List<IMvxViewTypeResolver> _resolvers;

        public MvxCompositeViewTypeResolver(params IMvxViewTypeResolver[] resolvers)
        {
            _resolvers = new List<IMvxViewTypeResolver>(resolvers);
        }

        public Type Resolve(string tagName)
        {
            foreach (var resolver in _resolvers)
            {
                var result = resolver.Resolve(tagName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
