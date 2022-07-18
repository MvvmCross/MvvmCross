// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public class MvxJustNameViewTypeResolver : MvxReflectionViewTypeResolver
    {
        public MvxJustNameViewTypeResolver(IMvxTypeCache<View> typeCache) : base(typeCache)
        {
        }

        public override Type Resolve(string tagName)
        {
            // this resolver can't handle fully qualified tag names
            if (IsFullyQualified(tagName))
                return null;

            Type toReturn;
            TypeCache.NameCache.TryGetValue(tagName, out toReturn);
            return toReturn;
        }
    }
}
