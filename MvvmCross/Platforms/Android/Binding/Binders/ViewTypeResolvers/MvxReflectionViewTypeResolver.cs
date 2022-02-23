// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Views;
using MvvmCross.IoC;

namespace MvvmCross.Platforms.Android.Binding.Binders.ViewTypeResolvers
{
    public abstract class MvxReflectionViewTypeResolver : IMvxViewTypeResolver
    {
        protected IMvxTypeCache<View> TypeCache { get; }

        protected MvxReflectionViewTypeResolver(IMvxTypeCache<View> typeCache)
        {
            TypeCache = typeCache;
        }

        public abstract Type Resolve(string tagName);

        protected static bool IsFullyQualified(string tagName)
        {
            return tagName.Contains(".");
        }
    }
}
