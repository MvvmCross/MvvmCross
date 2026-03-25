// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public class MvxJustNameViewTypeResolver : MvxReflectionViewTypeResolver
    {
        public MvxJustNameViewTypeResolver(IMvxViewTypeRegistry registry) : base(registry)
        {
        }

        [return: System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)]
        public override Type Resolve(string tagName)
        {
            // this resolver can't handle fully qualified tag names
            if (IsFullyQualified(tagName))
                return null;

            Registry.TryResolve(tagName, out var type);
            return type;
        }
    }
}
