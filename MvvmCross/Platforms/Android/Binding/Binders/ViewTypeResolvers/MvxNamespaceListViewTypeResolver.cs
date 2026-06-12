// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public class MvxNamespaceListViewTypeResolver : MvxLongLowerCaseViewTypeResolver, IMvxNamespaceListViewTypeResolver
    {
        public IList<string> Namespaces { get; }

        public MvxNamespaceListViewTypeResolver(IMvxViewTypeRegistry registry)
            : base(registry)
        {
            Namespaces = new List<string>();
        }

        public void Add(string namespaceName)
        {
            namespaceName = namespaceName.ToLower();
            if (!namespaceName.EndsWith('.'))
                namespaceName += '.';

            Namespaces.Add(namespaceName);
        }

        [return: System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public override Type? Resolve(string tagName)
        {
            // this resolver only handles simple namespaceless tagNames
            if (tagName.Contains('.'))
                return null;

            var lowerTagName = tagName.ToLower();
            foreach (var ns in Namespaces)
            {
                var candidateName = ns + lowerTagName;
                if (Registry.TryResolve(candidateName, out var type))
                    return type;
            }

            return null;
        }
    }
}
