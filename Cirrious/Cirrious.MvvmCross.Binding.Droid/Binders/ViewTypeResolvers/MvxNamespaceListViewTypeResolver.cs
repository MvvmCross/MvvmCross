// MvxNamespaceListViewTypeResolver.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Cirrious.CrossCore.IoC;

namespace Cirrious.MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public class MvxNamespaceListViewTypeResolver : MvxLongLowerCaseViewTypeResolver
    {
        public IList<string> Namespaces { get; set; }

        public MvxNamespaceListViewTypeResolver(IMvxTypeCache<View> typeCache)
            : base(typeCache)
        {
            Namespaces = new List<string>();
        }

        public void EnsureAllNamespacesAreLowerCaseAndEndWithPeriod()
        {
            Namespaces = Namespaces
                .Select(x => x.ToLower())
                .Select(x => x.EndsWith(".") ? x : x + ".")
                .ToList();
        }

        public override Type Resolve(string tagName)
        {
            // this resolver only handles simple namespaceless tagNames
            if (tagName.Contains("."))
                return null;

            var lowerTagName = tagName.ToLower();
            foreach (var ns in Namespaces)
            {
                var candidateName = ns + lowerTagName;
                Type type;
                if (TypeCache.LowerCaseFullNameCache.TryGetValue(candidateName, out type))
                    return type;
            }

            return null;
        }
    }
}