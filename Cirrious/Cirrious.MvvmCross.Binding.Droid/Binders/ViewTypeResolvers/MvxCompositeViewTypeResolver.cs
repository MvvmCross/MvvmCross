// MvxCompositeViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
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