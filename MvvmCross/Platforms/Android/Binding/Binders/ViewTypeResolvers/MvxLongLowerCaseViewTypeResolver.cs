// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Android.Views;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public abstract class MvxLongLowerCaseViewTypeResolver : MvxReflectionViewTypeResolver
    {
        protected MvxLongLowerCaseViewTypeResolver(IMvxViewTypeRegistry registry)
            : base(registry)
        {
        }

        [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        protected Type ResolveLowerCaseTypeName(string longLowerCaseName)
        {
            Registry.TryResolve(longLowerCaseName, out var type);
            return type!;
        }
    }
}
