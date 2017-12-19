﻿// MvxNamespaceListViewTypeResolver.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Android.Views;
using MvvmCross.Platform.IoC;

namespace MvvmCross.Binding.Droid.Binders.ViewTypeResolvers
{
    public class MvxNamespaceListViewTypeResolver : MvxLongLowerCaseViewTypeResolver, IMvxNamespaceListViewTypeResolver
    {
        public IList<string> Namespaces { get; }

        public MvxNamespaceListViewTypeResolver(IMvxTypeCache<View> typeCache)
            : base(typeCache)
        {
            Namespaces = new List<string>();
        }

        public void Add(string namespaceName)
        {
            namespaceName = namespaceName.ToLower();
            if (!namespaceName.EndsWith("."))
                namespaceName += ".";

            Namespaces.Add(namespaceName);
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