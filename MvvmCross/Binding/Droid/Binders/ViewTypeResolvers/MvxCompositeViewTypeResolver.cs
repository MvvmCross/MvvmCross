// MvxCompositeViewTypeResolver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    using System;
    using System.Collections.Generic;

    public class MvxCompositeViewTypeResolver : IMvxViewTypeResolver
    {
        private readonly List<IMvxViewTypeResolver> _resolvers;

        public MvxCompositeViewTypeResolver(params IMvxViewTypeResolver[] resolvers)
        {
            this._resolvers = new List<IMvxViewTypeResolver>(resolvers);
        }

        public Type Resolve(string tagName)
        {
            foreach (var resolver in this._resolvers)
            {
                var result = resolver.Resolve(tagName);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}